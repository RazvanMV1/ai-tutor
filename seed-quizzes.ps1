# === CONFIG ===
$baseUrl       = "http://localhost:5000"
$teacherEmail  = "test.teacher.27512@example.com"
$teacherPass   = "Teacher123!@"
$classroomId   = $null
$lessonId      = $null

# === 1. LOGIN ===
$loginBody = @{ email = $teacherEmail; password = $teacherPass } | ConvertTo-Json
$login = Invoke-RestMethod -Uri "$baseUrl/api/Auth/login" -Method Post `
            -ContentType "application/json" -Body $loginBody
$token     = $login.token
$teacherId = $login.user.id
$headers   = @{ Authorization = "Bearer $token" }
Write-Host "OK - Logat ca profesor: $teacherId" -ForegroundColor Green

# === 2. ALEGE CLASA ===
if (-not $classroomId) {
    $classes = Invoke-RestMethod -Uri "$baseUrl/api/Classrooms/teacher/$teacherId" -Headers $headers
    if (-not $classes -or $classes.Count -eq 0) { Write-Error "Nu exista clase."; return }
    $classroomId = $classes[0].id
    Write-Host "Clasa: $($classes[0].name) ($classroomId)" -ForegroundColor Cyan
}

# === 3. ALEGE LECTIA ===
if (-not $lessonId) {
    $lessons = Invoke-RestMethod -Uri "$baseUrl/api/Classrooms/$classroomId/lessons?userId=$teacherId" -Headers $headers
    if (-not $lessons -or $lessons.Count -eq 0) { Write-Error "Nu exista lectii. Creeaza una in UI."; return }
    $lessonId = $lessons[0].id
    Write-Host "Lectie: $($lessons[0].title) ($lessonId)" -ForegroundColor Cyan
}

# === 4. DEFINIRE QUIZ-URI ===
$quizzes = @(
    @{
        Title = "Quiz 1 - Recapitulare"
        Difficulty = 1
        TimeLimitMinutes = 15
        Questions = @(
            @{ Text="Cat fac 2+2?";  Options=@("3","4","5","6");      CorrectIndex=1 },
            @{ Text="Cat fac 5x3?";  Options=@("8","12","15","20");   CorrectIndex=2 },
            @{ Text="Cat fac 10-7?"; Options=@("2","3","4","5");      CorrectIndex=1 },
            @{ Text="Cat fac 9/3?";  Options=@("2","3","4","6");      CorrectIndex=1 }
        )
    },
    @{
        Title = "Quiz 2 - Mediu"
        Difficulty = 2
        TimeLimitMinutes = 20
        Questions = @(
            @{ Text="Cat fac 12x11?";    Options=@("121","131","132","144"); CorrectIndex=2 },
            @{ Text="Cat fac 144/12?";   Options=@("10","11","12","14");     CorrectIndex=2 },
            @{ Text="Cat e 7 la patrat?";Options=@("42","48","49","56");     CorrectIndex=2 },
            @{ Text="Cat e 25% din 80?"; Options=@("15","20","25","40");     CorrectIndex=1 }
        )
    },
    @{
        Title = "Quiz 3 - Avansat"
        Difficulty = 3
        TimeLimitMinutes = 30
        Questions = @(
            @{ Text="Cat e radical din 169?"; Options=@("11","12","13","14");        CorrectIndex=2 },
            @{ Text="Cat e 2 la 10?";         Options=@("512","1000","1024","2048"); CorrectIndex=2 },
            @{ Text="Cat e 15% din 240?";     Options=@("24","30","36","40");        CorrectIndex=2 },
            @{ Text="Cat fac 7! / 5!?";       Options=@("12","30","42","56");        CorrectIndex=2 }
        )
    }
)

# === 5. CREARE QUIZ-URI + INTREBARI ===
foreach ($q in $quizzes) {
    $quizBody = @{
        title            = $q.Title
        difficulty       = $q.Difficulty
        timeLimitMinutes = $q.TimeLimitMinutes
    } | ConvertTo-Json

    $quizUrl = "$baseUrl/api/Classrooms/$classroomId/lessons/$lessonId/quizzes?teacherId=$teacherId"
    try {
        $createdQuiz = Invoke-RestMethod -Uri $quizUrl -Method Post -Headers $headers -ContentType "application/json" -Body $quizBody
        $quizId = $createdQuiz.id
        Write-Host "  OK Quiz creat: $($q.Title) ($quizId)" -ForegroundColor Green
    } catch {
        Write-Host "  ERR Quiz '$($q.Title)': $($_.Exception.Message)" -ForegroundColor Red
        continue
    }

    foreach ($question in $q.Questions) {
        $qBody = @{
            text         = $question.Text
            options      = $question.Options
            correctIndex = $question.CorrectIndex
        } | ConvertTo-Json

        $qUrl = "$baseUrl/api/Classrooms/$classroomId/lessons/any/quizzes/$quizId/questions?teacherId=$teacherId"
        try {
            Invoke-RestMethod -Uri $qUrl -Method Post -Headers $headers -ContentType "application/json" -Body $qBody | Out-Null
            Write-Host "     - $($question.Text)" -ForegroundColor DarkGray
        } catch {
            Write-Host "     ERR intrebare: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

Write-Host ""
Write-Host "Done! Reincarca UI-ul pe lectia $lessonId." -ForegroundColor Green
