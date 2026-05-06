# scripts/run-coverage.ps1
param(
    [string]$SonarToken = $env:SONAR_LOCAL_TOKEN
)

if (-not $SonarToken) {
    Write-Error "Set `$env:SONAR_LOCAL_TOKEN sau pasează -SonarToken"
    exit 1
}

$env:JAVA_HOME = "C:\Program Files\Microsoft\jdk-17.0.18.8-hotspot"
$env:PATH = "$env:JAVA_HOME\bin;$env:PATH"

Remove-Item -Recurse -Force coverage, .sonarqube -ErrorAction SilentlyContinue

dotnet sonarscanner begin `
  /k:"AiTutor" /n:"AiTutor" `
  /d:sonar.host.url="http://localhost:9000" `
  /d:sonar.login="$SonarToken" `
  /d:sonar.cs.opencover.reportsPaths="coverage/coverage.opencover.xml" `
  /d:sonar.coverage.exclusions="**/Migrations/**,**/Program.cs,**/*.Designer.cs,**/wwwroot/**,**/SeedData.cs,**/Configurations/*Configuration.cs,**/DependencyInjection.cs,**/ai-module/**,**/Controllers/**,**/frontend/**,**/AiTutor.Web/**,**/StripeService.cs,**/AiTutorService.cs,**/ApplicationDbContext.cs,**/Identity/**,**/StripeOptions.cs" `
  /d:sonar.exclusions="**/Migrations/**,**/bin/**,**/obj/**,**/wwwroot/lib/**"

dotnet build AiTutor.slnx --no-incremental
dotnet test tests\AiTutor.UnitTests\AiTutor.UnitTests.csproj `
  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover `
  /p:CoverletOutput="../../coverage/coverage.opencover.xml" `
  /p:Exclude="[*.Tests]*"

dotnet sonarscanner end /d:sonar.login="$SonarToken"
Start-Process "http://localhost:9000/dashboard?id=ai-tutor"
