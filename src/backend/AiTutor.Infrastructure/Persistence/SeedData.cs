using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager)
    {
        if (await db.Users.AnyAsync())
            return;

        // ═══════════════════════════════════════════════════════════════
        // 1. SUBJECTS
        // ═══════════════════════════════════════════════════════════════
        var math = Subject.Create("Matematică",
            "Aritmetică, algebră, geometrie și analiză matematică pentru toate nivelurile.",
            SubjectType.Mathematics);
        var romanian = Subject.Create("Limba Română",
            "Gramatică, lectură, scriere creativă și literatura română.",
            SubjectType.Romanian);
        var info = Subject.Create("Informatică",
            "Algoritmi, programare, structuri de date și gândire computațională.",
            SubjectType.Informatics);

        db.Subjects.AddRange(math, romanian, info);
        await db.SaveChangesAsync();

        // ═══════════════════════════════════════════════════════════════
        // 2. USERS
        // ═══════════════════════════════════════════════════════════════
        var admin = User.Create("Admin", "Platform", "admin@aitutor.com", UserRole.Admin);
        var teacher1 = User.Create("Maria", "Popescu", "teacher1@aitutor.com", UserRole.Teacher);
        var teacher2 = User.Create("Ion", "Ionescu", "teacher2@aitutor.com", UserRole.Teacher);
        var teacher3 = User.Create("Elena", "Vasilescu", "teacher3@aitutor.com", UserRole.Teacher);
        var parent1 = User.Create("Elena", "Dumitrescu", "parent1@aitutor.com", UserRole.Parent);
        var parent2 = User.Create("Andrei", "Marin", "parent2@aitutor.com", UserRole.Parent);
        var parent3 = User.Create("Cristina", "Georgescu", "parent3@aitutor.com", UserRole.Parent);
        var student1 = User.Create("Alex", "Dumitrescu", "student1@aitutor.com", UserRole.Student);
        var student2 = User.Create("Ana", "Popa", "student2@aitutor.com", UserRole.Student);
        var student3 = User.Create("Mihai", "Radu", "student3@aitutor.com", UserRole.Student);
        var student4 = User.Create("Ioana", "Stan", "student4@aitutor.com", UserRole.Student);
        var student5 = User.Create("David", "Marin", "student5@aitutor.com", UserRole.Student);
        var student6 = User.Create("Sofia", "Georgescu", "student6@aitutor.com", UserRole.Student);
        var student7 = User.Create("Luca", "Constantinescu", "student7@aitutor.com", UserRole.Student);
        var student8 = User.Create("Maria", "Florescu", "student8@aitutor.com", UserRole.Student);

        var allUsers = new[] { admin, teacher1, teacher2, teacher3, parent1, parent2, parent3,
            student1, student2, student3, student4, student5, student6, student7, student8 };
        db.Users.AddRange(allUsers);
        await db.SaveChangesAsync();

        // Identity users
        var userCredentials = new (User domainUser, string password)[]
        {
            (admin, "Admin123!@"),
            (teacher1, "Teacher123!@"), (teacher2, "Teacher123!@"), (teacher3, "Teacher123!@"),
            (parent1, "Parent123!@"), (parent2, "Parent123!@"), (parent3, "Parent123!@"),
            (student1, "Student123!@"), (student2, "Student123!@"), (student3, "Student123!@"),
            (student4, "Student123!@"), (student5, "Student123!@"), (student6, "Student123!@"),
            (student7, "Student123!@"), (student8, "Student123!@"),
        };

        foreach (var (domainUser, password) in userCredentials)
        {
            var identityUser = new ApplicationUser
            {
                UserName = domainUser.Email.Value,
                Email = domainUser.Email.Value,
                DomainUserId = domainUser.Id,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(identityUser, password);
        }

        // ═══════════════════════════════════════════════════════════════
        // 3. SUBSCRIPTIONS
        // ═══════════════════════════════════════════════════════════════
        db.Subscriptions.AddRange(
            Subscription.Create(teacher1.Id, SubscriptionType.School, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(11), 49.99m),
            Subscription.Create(teacher2.Id, SubscriptionType.School, DateTime.UtcNow.AddMonths(-2), DateTime.UtcNow.AddMonths(10), 49.99m),
            Subscription.Create(teacher3.Id, SubscriptionType.School, DateTime.UtcNow.AddMonths(-3), DateTime.UtcNow.AddMonths(9), 49.99m),
            Subscription.Create(parent1.Id, SubscriptionType.ParentMonthly, DateTime.UtcNow.AddDays(-15), DateTime.UtcNow.AddDays(15), 9.99m),
            Subscription.Create(parent2.Id, SubscriptionType.ParentYearly, DateTime.UtcNow.AddMonths(-3), DateTime.UtcNow.AddMonths(9), 89.99m),
            Subscription.Create(parent3.Id, SubscriptionType.ParentMonthly, DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(25), 9.99m)
        );
        await db.SaveChangesAsync();

        // ═══════════════════════════════════════════════════════════════
        // 4. LESSONS — MATEMATICĂ (8 lecții, toate nivelurile)
        // ═══════════════════════════════════════════════════════════════
        var mathL1 = Lesson.Create("Numere naturale și operații",
            "Numerele naturale sunt: 0, 1, 2, 3, ...\n\n" +
            "Operații fundamentale:\n" +
            "• Adunarea: 15 + 23 = 38\n" +
            "• Scăderea: 45 - 18 = 27\n" +
            "• Înmulțirea: 7 × 8 = 56\n" +
            "• Împărțirea: 72 ÷ 9 = 8\n\n" +
            "Proprietăți ale adunării:\n" +
            "- Comutativitate: a + b = b + a\n" +
            "- Asociativitate: (a + b) + c = a + (b + c)\n" +
            "- Element neutru: a + 0 = a\n\n" +
            "Ordinea operațiilor (PEMDAS): Paranteze → Exponenți → Înmulțire/Împărțire → Adunare/Scădere",
            1, DifficultyLevel.Beginner, math.Id);

        var mathL2 = Lesson.Create("Fracții și numere zecimale",
            "O fracție reprezintă o parte dintr-un întreg: a/b, unde b ≠ 0.\n\n" +
            "Tipuri de fracții:\n" +
            "• Fracții proprii: 2/5 (numărător < numitor)\n" +
            "• Fracții improprii: 7/3 (numărător > numitor)\n" +
            "• Fracții echivalente: 1/2 = 2/4 = 3/6\n\n" +
            "Operații cu fracții:\n" +
            "• Adunare cu același numitor: 2/7 + 3/7 = 5/7\n" +
            "• Adunare cu numitori diferiți: 1/3 + 1/4 = 4/12 + 3/12 = 7/12\n" +
            "• Înmulțire: 2/3 × 4/5 = 8/15\n" +
            "• Împărțire: 2/3 ÷ 4/5 = 2/3 × 5/4 = 10/12 = 5/6\n\n" +
            "Conversie la zecimale: 3/4 = 0.75, 1/3 = 0.333...",
            2, DifficultyLevel.Beginner, math.Id);

        var mathL3 = Lesson.Create("Ecuații de gradul I",
            "O ecuație de gradul I are forma ax + b = 0, unde a ≠ 0.\n\n" +
            "Pași de rezolvare:\n" +
            "1. Se elimină parantezele (dacă există)\n" +
            "2. Se grupează termenii cu x în membrul stâng\n" +
            "3. Se grupează termenii liberi în membrul drept\n" +
            "4. Se împarte la coeficientul lui x\n\n" +
            "Exemple rezolvate:\n" +
            "• 2x + 6 = 0 → 2x = -6 → x = -3\n" +
            "• 3(x - 2) = x + 4 → 3x - 6 = x + 4 → 2x = 10 → x = 5\n" +
            "• 5x - 3 = 2x + 9 → 3x = 12 → x = 4\n\n" +
            "Verificare: înlocuim x în ecuația inițială și verificăm egalitatea.",
            3, DifficultyLevel.Beginner, math.Id);

        var mathL4 = Lesson.Create("Ecuații de gradul II",
            "Ecuația de gradul II are forma generală: ax² + bx + c = 0, unde a ≠ 0.\n\n" +
            "Metoda discriminantului:\n" +
            "1. Se calculează Δ = b² - 4ac\n" +
            "2. Dacă Δ > 0: x₁,₂ = (-b ± √Δ) / 2a (două soluții distincte)\n" +
            "3. Dacă Δ = 0: x = -b / 2a (soluție dublă)\n" +
            "4. Dacă Δ < 0: ecuația nu are soluții reale\n\n" +
            "Exemplu: x² - 5x + 6 = 0\n" +
            "a = 1, b = -5, c = 6\n" +
            "Δ = 25 - 24 = 1 > 0\n" +
            "x₁ = (5 + 1) / 2 = 3\n" +
            "x₂ = (5 - 1) / 2 = 2\n\n" +
            "Relațiile lui Viète: x₁ + x₂ = -b/a, x₁ · x₂ = c/a",
            4, DifficultyLevel.Intermediate, math.Id);

        var mathL5 = Lesson.Create("Funcții liniare și grafice",
            "O funcție liniară are forma f(x) = ax + b.\n\n" +
            "Elementele graficului:\n" +
            "• Panta (coeficientul unghiular) = a\n" +
            "  - a > 0: funcția este crescătoare\n" +
            "  - a < 0: funcția este descrescătoare\n" +
            "  - a = 0: funcția este constantă\n" +
            "• Ordonata la origine = b (punctul (0, b))\n" +
            "• Intersecția cu Ox: f(x) = 0 → x = -b/a → punctul (-b/a, 0)\n\n" +
            "Exemple:\n" +
            "• f(x) = 2x + 3: pantă pozitivă, trece prin (0, 3)\n" +
            "• f(x) = -x + 5: pantă negativă, trece prin (0, 5)\n\n" +
            "Două drepte sunt paralele dacă au aceeași pantă: a₁ = a₂\n" +
            "Două drepte sunt perpendiculare dacă a₁ · a₂ = -1",
            5, DifficultyLevel.Intermediate, math.Id);

        var mathL6 = Lesson.Create("Geometrie: Teorema lui Pitagora și aplicații",
            "În orice triunghi dreptunghic: a² + b² = c² (c = ipotenuza).\n\n" +
            "Triplete pitagoreice frecvente:\n" +
            "• (3, 4, 5) și multiplii: (6, 8, 10), (9, 12, 15)\n" +
            "• (5, 12, 13)\n" +
            "• (8, 15, 17)\n\n" +
            "Aplicații practice:\n" +
            "1. Diagonala unui dreptunghi cu laturile a și b: d = √(a² + b²)\n" +
            "   Ex: dreptunghi 6×8 → d = √(36+64) = √100 = 10\n\n" +
            "2. Înălțimea unui triunghi echilateral cu latura a: h = a√3/2\n" +
            "   Ex: latura 6 → h = 6√3/2 = 3√3 ≈ 5.196\n\n" +
            "3. Distanța dintre două puncte A(x₁,y₁) și B(x₂,y₂):\n" +
            "   d = √((x₂-x₁)² + (y₂-y₁)²)",
            6, DifficultyLevel.Intermediate, math.Id);

        var mathL7 = Lesson.Create("Sisteme de ecuații liniare",
            "Un sistem de ecuații liniare cu 2 necunoscute:\n" +
            "  a₁x + b₁y = c₁\n" +
            "  a₂x + b₂y = c₂\n\n" +
            "Metode de rezolvare:\n\n" +
            "1. SUBSTITUȚIA:\n" +
            "   - Se exprimă o necunoscută din prima ecuație\n" +
            "   - Se înlocuiește în a doua ecuație\n" +
            "   Ex: x + y = 5 și 2x - y = 1\n" +
            "   x = 5 - y → 2(5-y) - y = 1 → 10 - 3y = 1 → y = 3, x = 2\n\n" +
            "2. REDUCEREA:\n" +
            "   - Se înmulțesc ecuațiile cu constante potrivite\n" +
            "   - Se adună pentru a elimina o necunoscută\n\n" +
            "3. REGULA LUI CRAMER (determinanți):\n" +
            "   D = a₁b₂ - a₂b₁\n" +
            "   x = (c₁b₂ - c₂b₁) / D\n" +
            "   y = (a₁c₂ - a₂c₁) / D",
            7, DifficultyLevel.Advanced, math.Id);

        var mathL8 = Lesson.Create("Analiza matematică: Limite și derivate",
            "LIMITE:\n" +
            "Limita unei funcții f(x) când x → a este valoarea L la care se apropie f(x).\n" +
            "Notație: lim(x→a) f(x) = L\n\n" +
            "Limite fundamentale:\n" +
            "• lim(x→0) sin(x)/x = 1\n" +
            "• lim(x→∞) (1 + 1/x)^x = e ≈ 2.718\n\n" +
            "DERIVATE:\n" +
            "Derivata măsoară rata de variație a unei funcții.\n" +
            "f'(x) = lim(h→0) [f(x+h) - f(x)] / h\n\n" +
            "Derivate uzuale:\n" +
            "• (xⁿ)' = nxⁿ⁻¹\n" +
            "• (sin x)' = cos x\n" +
            "• (cos x)' = -sin x\n" +
            "• (eˣ)' = eˣ\n" +
            "• (ln x)' = 1/x\n\n" +
            "Reguli:\n" +
            "• (f + g)' = f' + g'\n" +
            "• (f · g)' = f'g + fg'\n" +
            "• (f/g)' = (f'g - fg') / g²",
            8, DifficultyLevel.Advanced, math.Id);

        // ═══════════════════════════════════════════════════════════════
        // 5. LESSONS — LIMBA ROMÂNĂ (8 lecții)
        // ═══════════════════════════════════════════════════════════════
        var romL1 = Lesson.Create("Părțile de vorbire flexibile",
            "Părțile de vorbire flexibile își schimbă forma prin declinare sau conjugare.\n\n" +
            "1. SUBSTANTIVUL — denumește ființe, obiecte, fenomene\n" +
            "   • Genuri: masculin (copil), feminin (carte), neutru (scaun)\n" +
            "   • Numere: singular (elev), plural (elevi)\n" +
            "   • Cazuri: N, G, D, Ac, V\n\n" +
            "2. ARTICOLUL — determină substantivul\n" +
            "   • Hotărât: -l, -le, -a (elevul, cartea)\n" +
            "   • Nehotărât: un, o, niște (un elev, o carte)\n\n" +
            "3. ADJECTIVUL — exprimă însușiri\n" +
            "   • Se acordă cu substantivul în gen, număr, caz\n" +
            "   • Grade de comparație: pozitiv, comparativ, superlativ\n\n" +
            "4. PRONUMELE — înlocuiește substantivul\n" +
            "   • Personal: eu, tu, el/ea\n" +
            "   • Posesiv: al meu, al tău\n" +
            "   • Demonstrativ: acesta, aceea\n\n" +
            "5. NUMERALUL — exprimă un număr\n" +
            "   • Cardinal: unu, doi, trei\n" +
            "   • Ordinal: primul, al doilea\n\n" +
            "6. VERBUL — exprimă acțiuni sau stări\n" +
            "   • Conjugări: I (-a), II (-ea), III (-e), IV (-î/-i)\n" +
            "   • Timpuri: prezent, imperfect, perfect simplu, perfect compus, viitor",
            1, DifficultyLevel.Beginner, romanian.Id);

        var romL2 = Lesson.Create("Părțile de vorbire neflexibile",
            "Părțile de vorbire neflexibile nu își schimbă forma.\n\n" +
            "1. ADVERBUL — determină un verb, adjectiv sau alt adverb\n" +
            "   • De loc: aici, acolo, sus, jos\n" +
            "   • De timp: azi, mâine, acum, atunci\n" +
            "   • De mod: bine, frumos, repede, încet\n" +
            "   • Grade de comparație (ca adjectivul)\n\n" +
            "2. PREPOZIȚIA — leagă cuvinte, cerând un anumit caz\n" +
            "   • Cu Ac: pe, la, în, prin, cu, despre, pentru\n" +
            "   • Cu G: asupra, contra, împotriva, deasupra\n" +
            "   • Cu D: datorită, conform, grație\n\n" +
            "3. CONJUNCȚIA — leagă propoziții sau părți de propoziție\n" +
            "   • Coordonatoare: și, dar, iar, sau, ori, deci, ci\n" +
            "   • Subordonatoare: că, dacă, deși, fiindcă, pentru că\n\n" +
            "4. INTERJECȚIA — exprimă sentimente, sunete\n" +
            "   • Vai! Bravo! Ah! Hei! Ura!\n" +
            "   • Se izolează prin virgulă sau semnul exclamării",
            2, DifficultyLevel.Beginner, romanian.Id);

        var romL3 = Lesson.Create("Analiza sintactică a propoziției",
            "Propoziția este o comunicare care conține un predicat.\n\n" +
            "PĂRȚI PRINCIPALE:\n\n" +
            "1. SUBIECTUL — cine face acțiunea?\n" +
            "   • Simplu: Copilul aleargă.\n" +
            "   • Multiplu: Maria și Ion citesc.\n" +
            "   • Subînțeles: Citesc o carte. (eu)\n" +
            "   • Inclus: Citește! (tu/el)\n\n" +
            "2. PREDICATUL — ce face subiectul?\n" +
            "   • Verbal: Copilul aleargă / a citit / va scrie.\n" +
            "   • Nominal: Cerul este albastru. (verb copulativ + NP)\n\n" +
            "PĂRȚI SECUNDARE:\n\n" +
            "3. ATRIBUTUL — determină un substantiv\n" +
            "   • Adjectival: cartea frumoasă\n" +
            "   • Substantival genitival: cartea elevului\n" +
            "   • Substantival prepozițional: carte de povești\n\n" +
            "4. COMPLEMENTUL — determină un verb\n" +
            "   • Direct: Citesc cartea. (ce?)\n" +
            "   • Indirect: Scriu mamei. (cui?)\n" +
            "   • Circumstanțial de loc: Merg acasă. (unde?)\n" +
            "   • Circumstanțial de timp: Vin mâine. (când?)\n" +
            "   • Circumstanțial de mod: Scrie frumos. (cum?)\n\n" +
            "5. NUMELE PREDICATIV — completează verbul copulativ\n" +
            "   • Cerul este albastru. (NP = albastru)",
            3, DifficultyLevel.Intermediate, romanian.Id);

        var romL4 = Lesson.Create("Fraza: propoziții principale și subordonate",
            "Fraza este formată din două sau mai multe propoziții.\n\n" +
            "PROPOZIȚIA PRINCIPALĂ (PP) — nu depinde de altă propoziție\n" +
            "PROPOZIȚIA SUBORDONATĂ (PS) — depinde de o altă propoziție\n\n" +
            "Tipuri de subordonate:\n\n" +
            "1. SUBIECTIVĂ (SB) — îndeplinește rolul de subiect\n" +
            "   • Cine învață reușește. (Cine? → SB)\n" +
            "   • E important să citești.\n\n" +
            "2. PREDICATIVĂ (PR) — îndeplinește rolul de NP\n" +
            "   • Întrebarea este dacă vom reuși.\n\n" +
            "3. ATRIBUTIVĂ (AT) — îndeplinește rolul de atribut\n" +
            "   • Elevul care citește mult reușește.\n\n" +
            "4. COMPLETIVĂ DIRECTĂ (CD) — complement direct\n" +
            "   • Știu că ai dreptate.\n\n" +
            "5. COMPLETIVĂ INDIRECTĂ (CI) — complement indirect\n" +
            "   • Mă gândesc la ce mi-ai spus.\n\n" +
            "6. CIRCUMSTANȚIALĂ DE TIMP (CT)\n" +
            "   • Când vine vara, plecăm la mare.\n\n" +
            "7. CIRCUMSTANȚIALĂ DE CAUZĂ (CC)\n" +
            "   • Am întârziat fiindcă a plouat.\n\n" +
            "8. CIRCUMSTANȚIALĂ CONDIȚIONALĂ (CND)\n" +
            "   • Dacă înveți, vei reuși.",
            4, DifficultyLevel.Intermediate, romanian.Id);

        var romL5 = Lesson.Create("Ion Creangă — Amintiri din copilărie",
            "AUTOR: Ion Creangă (1837-1889)\n" +
            "SPECIE: povestire autobiografică\n" +
            "PUBLICARE: 1881-1883, în revista \"Convorbiri literare\"\n\n" +
            "STRUCTURĂ: 4 părți narative\n\n" +
            "TEME PRINCIPALE:\n" +
            "• Copilăria ca vârstă de aur\n" +
            "• Natura și tradițiile rurale\n" +
            "• Educația (școala, familia)\n" +
            "• Trecerea de la copilărie la maturitate\n\n" +
            "PERSONAJE:\n" +
            "• Nică — naratorul, copil jucăuș și inventiv\n" +
            "• Smaranda (mama) — strictă dar iubitoare\n" +
            "• Bunica — blândă, protectoare\n" +
            "• David Creangă (tatăl) — meșter, muncitor\n\n" +
            "EPISOADE CELEBRE:\n" +
            "• Pupăza din tei\n" +
            "• Școala de la Broșteni\n" +
            "• Darea în brâci\n" +
            "• Jocurile copilăriei (de-a mijoarca)\n\n" +
            "STIL LITERAR:\n" +
            "• Limbaj popular, cu expresii și zicale\n" +
            "• Oralitate marcată\n" +
            "• Umor specific (auto-ironie)\n" +
            "• Detalii senzoriale bogate",
            5, DifficultyLevel.Beginner, romanian.Id);

        var romL6 = Lesson.Create("Mihai Eminescu — Luceafărul",
            "AUTOR: Mihai Eminescu (1850-1889)\n" +
            "SPECIE: poem filozofic\n" +
            "PUBLICARE: 1883, în \"Convorbiri literare\"\n\n" +
            "STRUCTURĂ: 4 tablouri (98 de strofe, 392 versuri)\n\n" +
            "TABLOUL I: Luceafărul se îndrăgostește de fata de împărat\n" +
            "TABLOUL II: Luceafărul coboară în formă omenească\n" +
            "TABLOUL III: Călătoria Luceafărului la Demiurg; cere dezlegare de nemurire\n" +
            "TABLOUL IV: Întoarcerea; fata alege un muritor; Luceafărul acceptă soarta\n\n" +
            "TEME:\n" +
            "• Geniul și incompatibilitatea cu lumea comună\n" +
            "• Iubirea absolută vs. iubirea terestră\n" +
            "• Condiția omului de geniu\n" +
            "• Cosmogonia (crearea lumii)\n\n" +
            "PERSONAJE-SIMBOL:\n" +
            "• Luceafărul — geniul, aspirația spre absolut\n" +
            "• Fata de împărat — umanitatea, frumusețea efemeră\n" +
            "• Cătălin — omul comun, pragmatic\n" +
            "• Demiurgul — creatorul suprem\n\n" +
            "CITAT CELEBRU:\n" +
            "\"Ce-ți pasă ție, chip de lut, / Dac-oi fi eu sau altul?\"",
            6, DifficultyLevel.Intermediate, romanian.Id);

        var romL7 = Lesson.Create("Figuri de stil și procedee artistice",
            "Figurile de stil sunt mijloace expresive ale limbajului literar.\n\n" +
            "FIGURI SEMANTICE (de sens):\n\n" +
            "1. EPITETUL — atribut expresiv\n" +
            "   • \"codri de aramă\" (Eminescu)\n" +
            "   • \"lacul codrilor albastru\"\n\n" +
            "2. COMPARAȚIA — asemănare explicită (cu \"ca\", \"precum\")\n" +
            "   • \"Ochii tăi sunt ca două stele\"\n\n" +
            "3. METAFORA — comparație implicită\n" +
            "   • \"viața e un drum lung\"\n\n" +
            "4. PERSONIFICAREA — atribuie însușiri umane unor obiecte/fenomene\n" +
            "   • \"Codrul cântă\" (Eminescu)\n\n" +
            "5. HIPERBOLA — exagerare expresivă\n" +
            "   • \"Mi-am pierdut mințile de bucurie\"\n\n" +
            "6. ALEGORIA — reprezentare a unei idei abstracte prin imagini concrete\n\n" +
            "FIGURI SINTACTICE:\n\n" +
            "7. INVERSIUNEA — modificarea ordinii firești a cuvintelor\n" +
            "   • \"Codri de aramă\" în loc de \"codri de aramă\"\n\n" +
            "8. REPETIȚIA — repetarea unui cuvânt/grup\n" +
            "   • \"Singur, singur pe pământ...\"\n\n" +
            "9. ENUMERAȚIA — înșiruire de cuvinte\n\n" +
            "10. EXCLAMAȚIA RETORICĂ\n" +
            "11. INTEROGAȚIA RETORICĂ",
            7, DifficultyLevel.Advanced, romanian.Id);

        var romL8 = Lesson.Create("Argumentarea — Eseu structurat",
            "Eseul structurat este o compunere argumentativă cu teză, argumente și concluzie.\n\n" +
            "STRUCTURA ESEULUI (5 paragrafe):\n\n" +
            "1. INTRODUCEREA (1 paragraf)\n" +
            "   • Prezentarea temei și a operei\n" +
            "   • Formularea tezei (opinia ta)\n" +
            "   • Ex: \"Romanul Ion de Liviu Rebreanu este o capodoperă a realismului românesc.\"\n\n" +
            "2. ARGUMENTUL 1 (1 paragraf)\n" +
            "   • Afirmația + explicația + citat/exemplu din text\n" +
            "   • Conectori: în primul rând, mai întâi, un prim argument\n\n" +
            "3. ARGUMENTUL 2 (1 paragraf)\n" +
            "   • Alt aspect + explicație + dovadă\n" +
            "   • Conectori: în al doilea rând, de asemenea, în plus\n\n" +
            "4. ARGUMENTUL 3 (opțional, 1 paragraf)\n" +
            "   • Conectori: nu în ultimul rând, totodată\n\n" +
            "5. CONCLUZIA (1 paragraf)\n" +
            "   • Reformularea tezei\n" +
            "   • Sinteză a argumentelor\n" +
            "   • Conectori: în concluzie, prin urmare, așadar\n\n" +
            "SFATURI:\n" +
            "• Minimum 400 de cuvinte (BAC)\n" +
            "• Folosește citate din text (între ghilimele)\n" +
            "• Menționează figuri de stil, teme, personaje\n" +
            "• Scrie la persoana I sau forma impersonală",
            8, DifficultyLevel.Advanced, romanian.Id);

        // ═══════════════════════════════════════════════════════════════
        // 6. LESSONS — INFORMATICĂ (8 lecții)
        // ═══════════════════════════════════════════════════════════════
        var infoL1 = Lesson.Create("Introducere în algoritmi",
            "Un algoritm este o succesiune finită și ordonată de pași care rezolvă o problemă.\n\n" +
            "PROPRIETĂȚI:\n" +
            "• Finitudine — se termină după un număr finit de pași\n" +
            "• Claritate — fiecare pas e precis definit\n" +
            "• Generalitate — rezolvă o clasă de probleme, nu doar un caz\n" +
            "• Eficiență — folosește resurse rezonabile\n\n" +
            "REPREZENTARE:\n" +
            "• Pseudocod (descriere text)\n" +
            "• Scheme logice (diagrame)\n" +
            "• Limbaj de programare (C++, Python, C#)\n\n" +
            "EXEMPLU — Suma primelor n numere:\n" +
            "  citește n\n" +
            "  S ← 0\n" +
            "  pentru i de la 1 la n execută\n" +
            "    S ← S + i\n" +
            "  scrie S\n\n" +
            "COMPLEXITATE:\n" +
            "• O(1) — constantă\n" +
            "• O(n) — liniară\n" +
            "• O(n²) — pătratică\n" +
            "• O(log n) — logaritmică",
            1, DifficultyLevel.Beginner, info.Id);

        var infoL2 = Lesson.Create("Variabile, tipuri de date și operatori",
            "VARIABILA — zonă de memorie cu un nume, care stochează o valoare.\n\n" +
            "TIPURI DE DATE FUNDAMENTALE (C#/C++):\n\n" +
            "• int — numere întregi: -2, 0, 42\n" +
            "  Exemplu: int varsta = 15;\n\n" +
            "• double/float — numere reale: 3.14, -0.5\n" +
            "  Exemplu: double medie = 8.75;\n\n" +
            "• string — text: \"Salut!\"\n" +
            "  Exemplu: string nume = \"Alex\";\n\n" +
            "• bool — adevărat/fals: true, false\n" +
            "  Exemplu: bool esteElev = true;\n\n" +
            "• char — un singur caracter: 'A', '5'\n" +
            "  Exemplu: char litera = 'B';\n\n" +
            "OPERATORI:\n" +
            "• Aritmetici: + - * / % (restul împărțirii)\n" +
            "• Relaționali: == != < > <= >=\n" +
            "• Logici: && (și), || (sau), ! (negare)\n" +
            "• Atribuire: = += -= *= /=\n\n" +
            "CONVERSII:\n" +
            "• Implicite: int → double (automat)\n" +
            "• Explicite: (int)3.7 → 3 (se pierde partea zecimală)",
            2, DifficultyLevel.Beginner, info.Id);

        var infoL3 = Lesson.Create("Structuri de decizie: if, else, switch",
            "Structurile de decizie permit executarea condiționată a instrucțiunilor.\n\n" +
            "IF-ELSE:\n" +
            "  if (condiție)\n" +
            "  {\n" +
            "      // se execută dacă condiția e adevărată\n" +
            "  }\n" +
            "  else\n" +
            "  {\n" +
            "      // se execută dacă condiția e falsă\n" +
            "  }\n\n" +
            "EXEMPLU — Verificare par/impar:\n" +
            "  if (n % 2 == 0)\n" +
            "      Console.WriteLine(\"Par\");\n" +
            "  else\n" +
            "      Console.WriteLine(\"Impar\");\n\n" +
            "IF-ELSE IF-ELSE (condiții multiple):\n" +
            "  if (nota >= 9) calificativ = \"Foarte bine\";\n" +
            "  else if (nota >= 7) calificativ = \"Bine\";\n" +
            "  else if (nota >= 5) calificativ = \"Suficient\";\n" +
            "  else calificativ = \"Insuficient\";\n\n" +
            "SWITCH:\n" +
            "  switch (ziua)\n" +
            "  {\n" +
            "      case 1: Console.WriteLine(\"Luni\"); break;\n" +
            "      case 2: Console.WriteLine(\"Marți\"); break;\n" +
            "      default: Console.WriteLine(\"Altă zi\"); break;\n" +
            "  }\n\n" +
            "OPERATORUL TERNAR: rezultat = (condiție) ? valoare1 : valoare2;",
            3, DifficultyLevel.Beginner, info.Id);

        var infoL4 = Lesson.Create("Structuri repetitive: for, while, do-while",
            "Structurile repetitive (bucle) execută un bloc de instrucțiuni de mai multe ori.\n\n" +
            "FOR — când știm numărul de iterații:\n" +
            "  for (int i = 1; i <= 10; i++)\n" +
            "  {\n" +
            "      Console.Write(i + \" \");\n" +
            "  }\n" +
            "  // Output: 1 2 3 4 5 6 7 8 9 10\n\n" +
            "WHILE — când NU știm dinainte câte iterații:\n" +
            "  int n = 12345;\n" +
            "  while (n > 0)\n" +
            "  {\n" +
            "      Console.Write(n % 10); // ultima cifră\n" +
            "      n = n / 10;\n" +
            "  }\n" +
            "  // Output: 5 4 3 2 1\n\n" +
            "DO-WHILE — se execută cel puțin o dată:\n" +
            "  do\n" +
            "  {\n" +
            "      Console.Write(\"Introdu un număr pozitiv: \");\n" +
            "      n = int.Parse(Console.ReadLine());\n" +
            "  } while (n <= 0);\n\n" +
            "PROBLEME CLASICE:\n" +
            "• Suma cifrelor unui număr\n" +
            "• Numărul de cifre\n" +
            "• Inversul unui număr\n" +
            "• Verificare palindrom\n" +
            "• CMMDC (algoritmul lui Euclid)",
            4, DifficultyLevel.Intermediate, info.Id);

        var infoL5 = Lesson.Create("Tablouri (Array-uri) unidimensionale",
            "Un array este o colecție de elemente de același tip, accesibile prin index.\n\n" +
            "DECLARARE (C#):\n" +
            "  int[] numere = new int[5];        // 5 elemente, inițializate cu 0\n" +
            "  int[] note = { 9, 10, 7, 8, 10 }; // inițializare directă\n\n" +
            "INDEXARE: de la 0 la length-1\n" +
            "  note[0] = 9  (primul element)\n" +
            "  note[4] = 10 (ultimul element)\n\n" +
            "PARCURGERE:\n" +
            "  for (int i = 0; i < note.Length; i++)\n" +
            "      Console.Write(note[i] + \" \");\n\n" +
            "PROBLEME FRECVENTE:\n\n" +
            "1. Suma elementelor:\n" +
            "   int suma = 0;\n" +
            "   for (int i = 0; i < n; i++) suma += a[i];\n\n" +
            "2. Minimul/Maximul:\n" +
            "   int max = a[0];\n" +
            "   for (int i = 1; i < n; i++)\n" +
            "       if (a[i] > max) max = a[i];\n\n" +
            "3. Căutare element:\n" +
            "   for (int i = 0; i < n; i++)\n" +
            "       if (a[i] == x) { gasit = true; break; }\n\n" +
            "4. Sortare (Bubble Sort):\n" +
            "   for (int i = 0; i < n-1; i++)\n" +
            "     for (int j = 0; j < n-i-1; j++)\n" +
            "       if (a[j] > a[j+1]) swap(a[j], a[j+1]);",
            5, DifficultyLevel.Intermediate, info.Id);

        var infoL6 = Lesson.Create("Matrice (Array-uri bidimensionale)",
            "O matrice este un array bidimensional cu linii și coloane.\n\n" +
            "DECLARARE (C#):\n" +
            "  int[,] mat = new int[3, 4]; // 3 linii, 4 coloane\n" +
            "  int[,] mat = { {1,2,3}, {4,5,6}, {7,8,9} };\n\n" +
            "PARCURGERE:\n" +
            "  for (int i = 0; i < linii; i++)\n" +
            "    for (int j = 0; j < coloane; j++)\n" +
            "      Console.Write(mat[i,j] + \" \");\n\n" +
            "MATRICE PĂTRATICĂ (n x n):\n\n" +
            "• Diagonala principală: i == j\n" +
            "• Diagonala secundară: i + j == n - 1\n" +
            "• Deasupra diag. principale: i < j\n" +
            "• Sub diag. principală: i > j\n\n" +
            "PROBLEME CLASICE:\n" +
            "1. Suma elementelor de pe diagonala principală\n" +
            "2. Transpusa matricei\n" +
            "3. Înmulțirea matricelor\n" +
            "4. Matrice simetrică (mat[i,j] == mat[j,i])\n" +
            "5. Spirala matricei",
            6, DifficultyLevel.Intermediate, info.Id);

        var infoL7 = Lesson.Create("Recursivitate",
            "O funcție recursivă se apelează pe ea însăși.\n\n" +
            "STRUCTURA UNEI FUNCȚII RECURSIVE:\n" +
            "1. Condiția de oprire (bază)\n" +
            "2. Apelul recursiv (se apropie de bază)\n\n" +
            "EXEMPLU — Factorial:\n" +
            "  int Factorial(int n)\n" +
            "  {\n" +
            "      if (n <= 1) return 1;        // bază\n" +
            "      return n * Factorial(n - 1);  // recursie\n" +
            "  }\n" +
            "  // Factorial(5) = 5 × 4 × 3 × 2 × 1 = 120\n\n" +
            "EXEMPLU — Fibonacci:\n" +
            "  int Fib(int n)\n" +
            "  {\n" +
            "      if (n <= 1) return n;\n" +
            "      return Fib(n-1) + Fib(n-2);\n" +
            "  }\n" +
            "  // 0, 1, 1, 2, 3, 5, 8, 13, 21...\n\n" +
            "EXEMPLU — CMMDC (Euclid recursiv):\n" +
            "  int Cmmdc(int a, int b)\n" +
            "  {\n" +
            "      if (b == 0) return a;\n" +
            "      return Cmmdc(b, a % b);\n" +
            "  }\n\n" +
            "STIVA DE APELURI:\n" +
            "Fiecare apel recursiv pune un cadru pe stivă.\n" +
            "Prea multe apeluri → StackOverflowException!\n\n" +
            "RECURSIE vs ITERAȚIE:\n" +
            "• Recursie: cod mai elegant, dar consumă memorie (stivă)\n" +
            "• Iterație: mai eficient, dar uneori cod mai complex",
            7, DifficultyLevel.Advanced, info.Id);

        var infoL8 = Lesson.Create("Programare orientată pe obiecte (OOP)",
            "OOP organizează codul în clase și obiecte.\n\n" +
            "CLASA — un șablon/plan care definește proprietăți și metode:\n" +
            "  class Elev\n" +
            "  {\n" +
            "      public string Nume { get; set; }\n" +
            "      public int Varsta { get; set; }\n" +
            "      public double Medie { get; set; }\n\n" +
            "      public void Prezinta()\n" +
            "      {\n" +
            "          Console.WriteLine($\"Sunt {Nume}, am {Varsta} ani\");\n" +
            "      }\n" +
            "  }\n\n" +
            "OBIECTUL — o instanță a clasei:\n" +
            "  Elev e = new Elev { Nume = \"Alex\", Varsta = 15, Medie = 9.5 };\n" +
            "  e.Prezinta();\n\n" +
            "CELE 4 PRINCIPII OOP:\n\n" +
            "1. ÎNCAPSULAREA — ascunderea detaliilor interne\n" +
            "   • Modificatori: public, private, protected\n" +
            "   • Proprietăți cu get/set\n\n" +
            "2. MOȘTENIREA — o clasă derivată preia de la clasa de bază\n" +
            "   • class Student : Elev { public string Facultate; }\n\n" +
            "3. POLIMORFISMUL — aceeași metodă, comportament diferit\n" +
            "   • virtual/override\n" +
            "   • Metode abstracte\n\n" +
            "4. ABSTRACȚIA — expunerea doar a esențialului\n" +
            "   • Clase abstracte\n" +
            "   • Interfețe (interface)",
            8, DifficultyLevel.Advanced, info.Id);

        var allLessons = new[] { mathL1, mathL2, mathL3, mathL4, mathL5, mathL6, mathL7, mathL8,
            romL1, romL2, romL3, romL4, romL5, romL6, romL7, romL8,
            infoL1, infoL2, infoL3, infoL4, infoL5, infoL6, infoL7, infoL8 };
        db.Lessons.AddRange(allLessons);
        await db.SaveChangesAsync();

        // ═══════════════════════════════════════════════════════════════
        // 7. QUIZZES + QUESTIONS (câte 2-3 quiz-uri per materie)
        // ═══════════════════════════════════════════════════════════════

        // ── MATEMATICĂ QUIZZES ─────────────────────────────────────────
        var qMath1 = Quiz.Create("Test: Numere și operații", DifficultyLevel.Beginner, mathL1.Id, 10);
        db.Quizzes.Add(qMath1); await db.SaveChangesAsync();
        db.Questions.AddRange(
            Question.Create("Cât face 15 + 23?", "38", new List<string> { "35", "38", "39", "28" }, 10, qMath1.Id, "15 + 23 = 38"),
            Question.Create("Cât face 72 ÷ 9?", "8", new List<string> { "7", "8", "9", "6" }, 10, qMath1.Id),
            Question.Create("Care este ordinea corectă a operațiilor?", "Paranteze, Înmulțire, Adunare", new List<string> { "Adunare, Înmulțire, Paranteze", "Paranteze, Înmulțire, Adunare", "Înmulțire, Adunare, Paranteze", "Nu contează ordinea" }, 10, qMath1.Id),
            Question.Create("Cât face 7 × 8?", "56", new List<string> { "54", "56", "58", "48" }, 10, qMath1.Id)
        );

        var qMath2 = Quiz.Create("Test: Ecuații de gradul I", DifficultyLevel.Beginner, mathL3.Id, 15);
        db.Quizzes.Add(qMath2); await db.SaveChangesAsync();
        db.Questions.AddRange(
            Question.Create("Care este soluția ecuației 3x + 9 = 0?", "-3", new List<string> { "-3", "3", "-9", "9" }, 10, qMath2.Id, "3x = -9 → x = -3"),
            Question.Create("Rezolvați: x - 5 = 10", "15", new List<string> { "5", "10", "15", "-5" }, 10, qMath2.Id),
            Question.Create("Care este forma generală a ecuației de gradul I?", "ax + b = 0", new List<string> { "ax² + bx + c = 0", "ax + b = 0", "a/x = b", "x = a" }, 10, qMath2.Id),
            Question.Create("Rezolvați: 5x - 3 = 2x + 9", "4", new List<string> { "2", "3", "4", "6" }, 10, qMath2.Id, "3x = 12 → x = 4"),
            Question.Create("Rezolvați: 2(x + 3) = 14", "4", new List<string> { "4", "5", "7", "3" }, 10, qMath2.Id, "2x + 6 = 14 → 2x = 8 → x = 4")
        );

        var qMath3 = Quiz.Create("Test: Ecuații de gradul II", DifficultyLevel.Intermediate, mathL4.Id, 20);
        db.Quizzes.Add(qMath3); await db.SaveChangesAsync();
        db.Questions.AddRange(
            Question.Create("Care este discriminantul ecuației x² - 5x + 6 = 0?", "1", new List<string> { "1", "0", "-1", "25" }, 10, qMath3.Id, "Δ = 25 - 24 = 1"),
            Question.Create("Câte soluții reale are ecuația dacă Δ < 0?", "0", new List<string> { "0", "1", "2", "infinit" }, 10, qMath3.Id),
            Question.Create("Soluțiile ecuației x² - 4 = 0 sunt:", "x = 2 și x = -2", new List<string> { "x = 2 și x = -2", "x = 4", "x = 2", "x = -4" }, 10, qMath3.Id),
            Question.Create("Formula discriminantului este:", "Δ = b² - 4ac", new List<string> { "Δ = b² - 4ac", "Δ = b² + 4ac", "Δ = 4ac - b²", "Δ = a² - 4bc" }, 10, qMath3.Id)
        );

        var qMath4 = Quiz.Create("Test: Limite și derivate", DifficultyLevel.Advanced, mathL8.Id, 25);
        db.Quizzes.Add(qMath4); await db.SaveChangesAsync();
        db.Questions.AddRange(
            Question.Create("Care este derivata funcției f(x) = x³?", "3x²", new List<string> { "3x²", "x²", "3x", "x³" }, 10, qMath4.Id, "(xⁿ)' = nxⁿ⁻¹"),
            Question.Create("Care este lim(x→0) sin(x)/x?", "1", new List<string> { "0", "1", "∞", "nu există" }, 10, qMath4.Id),
            Question.Create("Derivata funcției f(x) = eˣ este:", "eˣ", new List<string> { "eˣ", "xeˣ⁻¹", "1/x", "0" }, 10, qMath4.Id),
            Question.Create("Care este derivata lui sin(x)?", "cos(x)", new List<string> { "cos(x)", "-cos(x)", "sin(x)", "-sin(x)" }, 10, qMath4.Id)
        );

        // ── ROMÂNĂ QUIZZES ─────────────────────────────────────────────
        var qRom1 = Quiz.Create("Test: Părți de vorbire", DifficultyLevel.Beginner, romL1.Id, 15);
        db.Quizzes.Add(qRom1); await db.SaveChangesAsync();
        db.Questions.AddRange(
            Question.Create("'Frumos' este:", "Adjectiv", new List<string> { "Substantiv", "Adjectiv", "Verb", "Adverb" }, 10, qRom1.Id),
            Question.Create("Câte părți de vorbire flexibile există?", "6", new List<string> { "4", "5", "6", "8" }, 10, qRom1.Id),
            Question.Create("'Repede' este:", "Adverb", new List<string> { "Adjectiv", "Verb", "Adverb", "Prepoziție" }, 10, qRom1.Id),
            Question.Create("Care parte de vorbire denumește ființe și obiecte?", "Substantivul", new List<string> { "Substantivul", "Adjectivul", "Verbul", "Pronumele" }, 10, qRom1.Id),
            Question.Create("Articolul hotărât pentru 'carte' (feminin) este:", "-a (cartea)", new List<string> { "-ul", "-a (cartea)", "-le", "-lui" }, 10, qRom1.Id)
        );

        var qRom2 = Quiz.Create("Test: Analiza sintactică", DifficultyLevel.Intermediate, romL3.Id, 20);
        db.Quizzes.Add(qRom2); await db.SaveChangesAsync();
        db.Questions.AddRange(
            Question.Create("Subiectul răspunde la întrebarea:", "Cine? sau Ce?", new List<string> { "Cine? sau Ce?", "Ce face?", "Cum?", "Unde?" }, 10, qRom2.Id),
            Question.Create("'Cerul este albastru.' — 'albastru' este:", "Nume predicativ", new List<string> { "Atribut", "Complement direct", "Nume predicativ", "Predicat verbal" }, 10, qRom2.Id),
            Question.Create("Complementul direct răspunde la:", "Ce? Pe cine?", new List<string> { "Ce? Pe cine?", "Cui?", "Unde?", "Când?" }, 10, qRom2.Id),
            Question.Create("În 'Citesc cartea mamei', 'mamei' este:", "Atribut substantival genitival", new List<string> { "Complement indirect", "Atribut substantival genitival", "Subiect", "Complement direct" }, 10, qRom2.Id)
        );

        var qRom3 = Quiz.Create("Test: Figuri de stil", DifficultyLevel.Advanced, romL7.Id, 20);
        db.Quizzes.Add(qRom3); await db.SaveChangesAsync();
        db.Questions.AddRange(
            Question.Create("'Codri de aramă' este:", "Epitet", new List<string> { "Metaforă", "Comparație", "Epitet", "Personificare" }, 10, qRom3.Id),
            Question.Create("'Viața e un drum lung' este:", "Metaforă", new List<string> { "Epitet", "Comparație", "Metaforă", "Hiperbola" }, 10, qRom3.Id),
            Question.Create("'Codrul cântă' este:", "Personificare", new List<string> { "Metaforă", "Personificare", "Comparație", "Epitet" }, 10, qRom3.Id),
            Question.Create("Comparația folosește cuvinte precum:", "ca, precum, asemenea", new List<string> { "ca, precum, asemenea", "este, sunt", "dar, însă", "deoarece, fiindcă" }, 10, qRom3.Id),
            Question.Create("Exagerarea expresivă se numește:", "Hiperbolă", new List<string> { "Metaforă", "Hiperbolă", "Litotă", "Alegorie" }, 10, qRom3.Id)
        );

        // ── INFORMATICĂ QUIZZES ────────────────────────────────────────
        var qInfo1 = Quiz.Create("Test: Algoritmi de bază", DifficultyLevel.Beginner, infoL1.Id, 10);
        db.Quizzes.Add(qInfo1); await db.SaveChangesAsync();
        db.Questions.AddRange(
            Question.Create("Ce este un algoritm?", "O succesiune finită de pași", new List<string> { "Un program", "O succesiune finită de pași", "Un limbaj de programare", "O bază de date" }, 10, qInfo1.Id),
            Question.Create("Care NU este o proprietate a algoritmilor?", "Infinitudine", new List<string> { "Finitudine", "Claritate", "Infinitudine", "Generalitate" }, 10, qInfo1.Id),
            Question.Create("Complexitatea O(n) se numește:", "Liniară", new List<string> { "Constantă", "Liniară", "Pătratică", "Logaritmică" }, 10, qInfo1.Id),
            Question.Create("Cum se poate reprezenta un algoritm?", "Pseudocod sau scheme logice", new List<string> { "Doar în C++", "Pseudocod sau scheme logice", "Doar verbal", "Doar grafic" }, 10, qInfo1.Id)
        );

        var qInfo2 = Quiz.Create("Test: Variabile și tipuri de date", DifficultyLevel.Beginner, infoL2.Id, 10);
        db.Quizzes.Add(qInfo2); await db.SaveChangesAsync();
        db.Questions.AddRange(
            Question.Create("Ce tip de date stochează numere întregi?", "int", new List<string> { "string", "int", "bool", "double" }, 10, qInfo2.Id),
            Question.Create("Ce valori poate avea o variabilă bool?", "true sau false", new List<string> { "0 sau 1", "true sau false", "da sau nu", "orice text" }, 10, qInfo2.Id),
            Question.Create("'string' stochează:", "Text", new List<string> { "Numere", "Text", "Valori logice", "Caractere singulare" }, 10, qInfo2.Id),
            Question.Create("Operatorul % returnează:", "Restul împărțirii", new List<string> { "Câtul", "Restul împărțirii", "Produsul", "Suma" }, 10, qInfo2.Id)
        );

        var qInfo3 = Quiz.Create("Test: Bucle și array-uri", DifficultyLevel.Intermediate, infoL5.Id, 20);
        db.Quizzes.Add(qInfo3); await db.SaveChangesAsync();
        db.Questions.AddRange(
            Question.Create("Indexul primului element dintr-un array este:", "0", new List<string> { "0", "1", "-1", "depinde" }, 10, qInfo3.Id),
            Question.Create("Bucla 'for' se folosește când:", "Știm numărul de iterații", new List<string> { "Știm numărul de iterații", "Nu știm câte iterații", "Doar pentru array-uri", "Doar pentru numere" }, 10, qInfo3.Id),
            Question.Create("Bubble Sort are complexitatea:", "O(n²)", new List<string> { "O(n)", "O(n²)", "O(log n)", "O(1)" }, 10, qInfo3.Id),
            Question.Create("Ce buclă se execută cel puțin o dată?", "do-while", new List<string> { "for", "while", "do-while", "foreach" }, 10, qInfo3.Id),
            Question.Create("Array-ul int[] a = new int[5] are:", "5 elemente (index 0-4)", new List<string> { "5 elemente (index 0-4)", "5 elemente (index 1-5)", "6 elemente", "4 elemente" }, 10, qInfo3.Id)
        );

        var qInfo4 = Quiz.Create("Test: Recursivitate și OOP", DifficultyLevel.Advanced, infoL7.Id, 25);
        db.Quizzes.Add(qInfo4); await db.SaveChangesAsync();
        db.Questions.AddRange(
            Question.Create("Ce este recursivitatea?", "O funcție care se apelează pe ea însăși", new List<string> { "O buclă for", "O funcție care se apelează pe ea însăși", "O clasă abstractă", "O interfață" }, 10, qInfo4.Id),
            Question.Create("Factorial(0) este:", "1", new List<string> { "0", "1", "nedefinit", "∞" }, 10, qInfo4.Id),
            Question.Create("Care sunt cele 4 principii OOP?", "Încapsulare, Moștenire, Polimorfism, Abstracție", new List<string> { "Încapsulare, Moștenire, Polimorfism, Abstracție", "Clasă, Obiect, Metodă, Proprietate", "Public, Private, Protected, Internal", "Variabile, Funcții, Clase, Interfețe" }, 10, qInfo4.Id),
            Question.Create("Prea multe apeluri recursive cauzează:", "StackOverflowException", new List<string> { "NullPointerException", "StackOverflowException", "OutOfMemoryException", "TypeError" }, 10, qInfo4.Id),
            Question.Create("Moștenirea în C# se face cu:", ":", new List<string> { "extends", ":", "implements", "inherits" }, 10, qInfo4.Id)
        );

        await db.SaveChangesAsync();

        // ═══════════════════════════════════════════════════════════════
        // 8. STUDENT PROGRESS (divers)
        // ═══════════════════════════════════════════════════════════════
        var progresses = new List<StudentProgress>();

        // Student1 — mult progres
        var sp1 = StudentProgress.Create(student1.Id, mathL1.Id); sp1.Complete(95); progresses.Add(sp1);
        var sp2 = StudentProgress.Create(student1.Id, mathL2.Id); sp2.Complete(88); progresses.Add(sp2);
        var sp3 = StudentProgress.Create(student1.Id, mathL3.Id); sp3.Complete(92); progresses.Add(sp3);
        var sp4 = StudentProgress.Create(student1.Id, romL1.Id); sp4.Complete(85); progresses.Add(sp4);
        var sp5 = StudentProgress.Create(student1.Id, romL2.Id); sp5.Complete(78); progresses.Add(sp5);
        var sp6 = StudentProgress.Create(student1.Id, infoL1.Id); sp6.Complete(100); progresses.Add(sp6);
        var sp7 = StudentProgress.Create(student1.Id, infoL2.Id); sp7.Complete(90); progresses.Add(sp7);

        // Student2
        var sp8 = StudentProgress.Create(student2.Id, mathL1.Id); sp8.Complete(90); progresses.Add(sp8);
        var sp9 = StudentProgress.Create(student2.Id, mathL2.Id); sp9.Complete(75); progresses.Add(sp9);
        var sp10 = StudentProgress.Create(student2.Id, infoL1.Id); sp10.Complete(80); progresses.Add(sp10);
        var sp11 = StudentProgress.Create(student2.Id, romL1.Id); sp11.Complete(92); progresses.Add(sp11);

        // Student3
        var sp12 = StudentProgress.Create(student3.Id, romL1.Id); sp12.Complete(70); progresses.Add(sp12);
        var sp13 = StudentProgress.Create(student3.Id, romL2.Id); sp13.Complete(65); progresses.Add(sp13);
        var sp14 = StudentProgress.Create(student3.Id, romL3.Id); sp14.Complete(80); progresses.Add(sp14);
        var sp15 = StudentProgress.Create(student3.Id, mathL1.Id); sp15.Complete(60); progresses.Add(sp15);

        // Student4
        var sp16 = StudentProgress.Create(student4.Id, infoL1.Id); sp16.Complete(85); progresses.Add(sp16);
        var sp17 = StudentProgress.Create(student4.Id, infoL2.Id); sp17.Complete(78); progresses.Add(sp17);
        var sp18 = StudentProgress.Create(student4.Id, infoL3.Id); sp18.Complete(90); progresses.Add(sp18);

        // Student5
        var sp19 = StudentProgress.Create(student5.Id, mathL1.Id); sp19.Complete(55); progresses.Add(sp19);
        var sp20 = StudentProgress.Create(student5.Id, infoL1.Id); sp20.Complete(70); progresses.Add(sp20);

        // Student6, 7, 8 — progres parțial
        var sp21 = StudentProgress.Create(student6.Id, romL1.Id); sp21.Complete(88); progresses.Add(sp21);
        var sp22 = StudentProgress.Create(student6.Id, romL5.Id); sp22.Complete(92); progresses.Add(sp22);
        var sp23 = StudentProgress.Create(student7.Id, mathL1.Id); sp23.Complete(73); progresses.Add(sp23);
        var sp24 = StudentProgress.Create(student7.Id, mathL4.Id); sp24.Complete(68); progresses.Add(sp24);
        var sp25 = StudentProgress.Create(student8.Id, infoL1.Id); sp25.Complete(95); progresses.Add(sp25);
        var sp26 = StudentProgress.Create(student8.Id, infoL4.Id); sp26.Complete(82); progresses.Add(sp26);

        // Progres nefinalizat
        progresses.Add(StudentProgress.Create(student5.Id, mathL4.Id));
        progresses.Add(StudentProgress.Create(student6.Id, infoL1.Id));
        progresses.Add(StudentProgress.Create(student7.Id, romL1.Id));

        db.StudentProgresses.AddRange(progresses);
        await db.SaveChangesAsync();

        // ═══════════════════════════════════════════════════════════════
        // 9. CLASSROOMS (3 clase)
        // ═══════════════════════════════════════════════════════════════
        var classroom1 = Classroom.Create("Matematică — Clasa a 8-a",
            "Pregătire evaluare națională la matematică. Ecuații, geometrie, funcții.",
            SubjectType.Mathematics, teacher1.Id);
        var classroom2 = Classroom.Create("Informatică — Începători",
            "Curs introductiv de informatică: algoritmi, variabile, structuri de control.",
            SubjectType.Informatics, teacher2.Id);
        var classroom3 = Classroom.Create("Limba Română — Clasa a 8-a",
            "Pregătire evaluare națională: gramatică, analiză, literatură.",
            SubjectType.Romanian, teacher3.Id);

        db.Classrooms.AddRange(classroom1, classroom2, classroom3);
        await db.SaveChangesAsync();

        // ── Members ────────────────────────────────────────────────────
        db.ClassroomMembers.AddRange(
            // Classroom 1 — Matematică
            ClassroomMember.Create(classroom1.Id, student1.Id),
            ClassroomMember.Create(classroom1.Id, student2.Id),
            ClassroomMember.Create(classroom1.Id, student3.Id),
            ClassroomMember.Create(classroom1.Id, student7.Id),
            // Classroom 2 — Informatică
            ClassroomMember.Create(classroom2.Id, student1.Id),
            ClassroomMember.Create(classroom2.Id, student4.Id),
            ClassroomMember.Create(classroom2.Id, student5.Id),
            ClassroomMember.Create(classroom2.Id, student8.Id),
            // Classroom 3 — Română
            ClassroomMember.Create(classroom3.Id, student2.Id),
            ClassroomMember.Create(classroom3.Id, student3.Id),
            ClassroomMember.Create(classroom3.Id, student6.Id),
            ClassroomMember.Create(classroom3.Id, student7.Id),
            ClassroomMember.Create(classroom3.Id, student8.Id)
        );
        await db.SaveChangesAsync();

        // ── Classroom Lessons ──────────────────────────────────────────
        var cl1 = ClassroomLesson.Create(classroom1.Id, "Ecuații și inecuații",
            "Rezolvarea ecuațiilor de gradul I și II. Metode: substituție, discriminant.\nExercții practice cu verificare pas cu pas.",
            1, DifficultyLevel.Beginner);
        var cl2 = ClassroomLesson.Create(classroom1.Id, "Geometrie plană",
            "Triunghiuri, patrulatere, cercuri. Teorema lui Pitagora, aria, perimetrul.\nProbleme cu figuri compuse.",
            2, DifficultyLevel.Intermediate);
        var cl3 = ClassroomLesson.Create(classroom1.Id, "Funcții și grafice",
            "Funcții liniare și pătratice. Reprezentarea grafică, intersecții, monotonie.",
            3, DifficultyLevel.Advanced);

        var cl4 = ClassroomLesson.Create(classroom2.Id, "Variabile și tipuri de date",
            "Introducere în programare: ce este o variabilă, tipuri int, string, bool, double.\nDeclarare și inițializare.",
            1, DifficultyLevel.Beginner);
        var cl5 = ClassroomLesson.Create(classroom2.Id, "Structuri de decizie",
            "Instrucțiunea if-else, if-else if-else, switch-case.\nExemple practice.",
            2, DifficultyLevel.Beginner);
        var cl6 = ClassroomLesson.Create(classroom2.Id, "Bucle și array-uri",
            "for, while, do-while. Declararea și parcurgerea array-urilor.\nSortare, căutare, sume.",
            3, DifficultyLevel.Intermediate);

        var cl7 = ClassroomLesson.Create(classroom3.Id, "Părți de vorbire — Recapitulare",
            "Substantiv, adjectiv, verb, adverb, pronume. Exerciții de identificare și analiză gramaticală.",
            1, DifficultyLevel.Beginner);
        var cl8 = ClassroomLesson.Create(classroom3.Id, "Analiza sintactică",
            "Subiect, predicat, atribut, complement, nume predicativ. Analiză pe propoziții.",
            2, DifficultyLevel.Intermediate);
        var cl9 = ClassroomLesson.Create(classroom3.Id, "Eseu argumentativ",
            "Structura eseului: teză, argumente, concluzie. Exerciții pe texte literare.",
            3, DifficultyLevel.Advanced);

        db.ClassroomLessons.AddRange(cl1, cl2, cl3, cl4, cl5, cl6, cl7, cl8, cl9);
        await db.SaveChangesAsync();

        // ── Classroom Quizzes + Questions ──────────────────────────────
        var cq1 = ClassroomQuiz.Create(classroom1.Id, cl1.Id, "Test Ecuații", DifficultyLevel.Beginner, 15);
        var cq2 = ClassroomQuiz.Create(classroom1.Id, cl2.Id, "Test Geometrie", DifficultyLevel.Intermediate, 20);
        var cq3 = ClassroomQuiz.Create(classroom2.Id, cl4.Id, "Test Variabile", DifficultyLevel.Beginner, 10);
        var cq4 = ClassroomQuiz.Create(classroom2.Id, cl6.Id, "Test Bucle", DifficultyLevel.Intermediate, 15);
        var cq5 = ClassroomQuiz.Create(classroom3.Id, cl7.Id, "Test Părți de vorbire", DifficultyLevel.Beginner, 15);
        var cq6 = ClassroomQuiz.Create(classroom3.Id, cl8.Id, "Test Sintaxă", DifficultyLevel.Intermediate, 20);

        db.ClassroomQuizzes.AddRange(cq1, cq2, cq3, cq4, cq5, cq6);
        await db.SaveChangesAsync();

        db.ClassroomQuestions.AddRange(
            // cq1 — Ecuații
            ClassroomQuestion.Create(cq1.Id, "Rezolvați: 2x + 4 = 0", "-2", new List<string> { "-2", "2", "-4", "4" }, 10, "2x = -4 → x = -2"),
            ClassroomQuestion.Create(cq1.Id, "Rezolvați: x + 10 = 15", "5", new List<string> { "5", "10", "15", "25" }, 10, "x = 15 - 10 = 5"),
            ClassroomQuestion.Create(cq1.Id, "Tip ecuație: 3x - 7 = 0?", "Gradul I", new List<string> { "Gradul I", "Gradul II", "Inecuație", "Sistem" }, 10),
            ClassroomQuestion.Create(cq1.Id, "Rezolvați: 4x = 20", "5", new List<string> { "4", "5", "20", "80" }, 10),
            // cq2 — Geometrie
            ClassroomQuestion.Create(cq2.Id, "Într-un triunghi dreptunghic cu catetele 3 și 4, ipotenuza este:", "5", new List<string> { "5", "6", "7", "12" }, 10, "3² + 4² = 9 + 16 = 25, √25 = 5"),
            ClassroomQuestion.Create(cq2.Id, "Aria unui pătrat cu latura 6 este:", "36", new List<string> { "12", "24", "36", "48" }, 10),
            ClassroomQuestion.Create(cq2.Id, "Perimetrul unui dreptunghi 5×3 este:", "16", new List<string> { "8", "15", "16", "30" }, 10, "P = 2(5+3) = 16"),
            // cq3 — Variabile
            ClassroomQuestion.Create(cq3.Id, "Tip date pentru numere întregi:", "int", new List<string> { "string", "int", "bool", "double" }, 10),
            ClassroomQuestion.Create(cq3.Id, "bool poate fi:", "true sau false", new List<string> { "0 sau 1", "true sau false", "da sau nu", "orice" }, 10),
            ClassroomQuestion.Create(cq3.Id, "double stochează:", "Numere reale", new List<string> { "Text", "Numere întregi", "Numere reale", "Caractere" }, 10),
            // cq4 — Bucle
            ClassroomQuestion.Create(cq4.Id, "Bucla 'for' se folosește când:", "Știm numărul de iterații", new List<string> { "Știm numărul de iterații", "Nu știm", "Doar pentru stringuri", "Niciodată" }, 10),
            ClassroomQuestion.Create(cq4.Id, "Primul index al unui array este:", "0", new List<string> { "0", "1", "-1", "depinde" }, 10),
            ClassroomQuestion.Create(cq4.Id, "do-while se execută cel puțin:", "O dată", new List<string> { "Niciodată", "O dată", "De două ori", "Depinde" }, 10),
            // cq5 — Părți de vorbire
            ClassroomQuestion.Create(cq5.Id, "'Frumos' este:", "Adjectiv", new List<string> { "Substantiv", "Adjectiv", "Verb", "Adverb" }, 10),
            ClassroomQuestion.Create(cq5.Id, "'Elevul' conține articol:", "Hotărât", new List<string> { "Hotărât", "Nehotărât", "Nu conține", "Posesiv" }, 10),
            ClassroomQuestion.Create(cq5.Id, "Verbul exprimă:", "Acțiuni sau stări", new List<string> { "Însușiri", "Acțiuni sau stări", "Obiecte", "Numere" }, 10),
            // cq6 — Sintaxă
            ClassroomQuestion.Create(cq6.Id, "Subiectul răspunde la:", "Cine? Ce?", new List<string> { "Cine? Ce?", "Ce face?", "Cum?", "Unde?" }, 10),
            ClassroomQuestion.Create(cq6.Id, "'Cerul este albastru' — 'albastru' este:", "Nume predicativ", new List<string> { "Atribut", "Complement", "Nume predicativ", "Predicat" }, 10),
            ClassroomQuestion.Create(cq6.Id, "Complementul direct răspunde la:", "Ce? Pe cine?", new List<string> { "Ce? Pe cine?", "Cui?", "Unde?", "Când?" }, 10)
        );
        await db.SaveChangesAsync();

        // ── Classroom Progress ─────────────────────────────────────────
        var cpList = new List<ClassroomProgress>();

        var cp1 = ClassroomProgress.Create(classroom1.Id, student1.Id, cl1.Id); cp1.Complete(92); cpList.Add(cp1);
        var cp2 = ClassroomProgress.Create(classroom1.Id, student1.Id, cl2.Id); cp2.Complete(85); cpList.Add(cp2);
        var cp3 = ClassroomProgress.Create(classroom1.Id, student2.Id, cl1.Id); cp3.Complete(88); cpList.Add(cp3);
        var cp4 = ClassroomProgress.Create(classroom1.Id, student3.Id, cl1.Id); cpList.Add(cp4); // nefinalizat
        var cp5 = ClassroomProgress.Create(classroom1.Id, student7.Id, cl1.Id); cp5.Complete(75); cpList.Add(cp5);

        var cp6 = ClassroomProgress.Create(classroom2.Id, student1.Id, cl4.Id); cp6.Complete(100); cpList.Add(cp6);
        var cp7 = ClassroomProgress.Create(classroom2.Id, student4.Id, cl4.Id); cp7.Complete(80); cpList.Add(cp7);
        var cp8 = ClassroomProgress.Create(classroom2.Id, student4.Id, cl5.Id); cp8.Complete(75); cpList.Add(cp8);
        var cp9 = ClassroomProgress.Create(classroom2.Id, student5.Id, cl4.Id); cp9.Complete(65); cpList.Add(cp9);
        var cp10 = ClassroomProgress.Create(classroom2.Id, student8.Id, cl4.Id); cp10.Complete(95); cpList.Add(cp10);

        var cp11 = ClassroomProgress.Create(classroom3.Id, student2.Id, cl7.Id); cp11.Complete(90); cpList.Add(cp11);
        var cp12 = ClassroomProgress.Create(classroom3.Id, student3.Id, cl7.Id); cp12.Complete(72); cpList.Add(cp12);
        var cp13 = ClassroomProgress.Create(classroom3.Id, student6.Id, cl7.Id); cp13.Complete(88); cpList.Add(cp13);
        var cp14 = ClassroomProgress.Create(classroom3.Id, student6.Id, cl8.Id); cp14.Complete(82); cpList.Add(cp14);

        db.ClassroomProgresses.AddRange(cpList);
        await db.SaveChangesAsync();

        // ── Grades ─────────────────────────────────────────────────────
        db.Grades.AddRange(
            // Classroom 1 — Matematică (teacher1)
            Grade.Create(classroom1.Id, student1.Id, teacher1.Id, 9, "Foarte bine la ecuații"),
            Grade.Create(classroom1.Id, student1.Id, teacher1.Id, 8, "Bine la geometrie"),
            Grade.Create(classroom1.Id, student2.Id, teacher1.Id, 10, "Excelent!"),
            Grade.Create(classroom1.Id, student2.Id, teacher1.Id, 9, "Foarte bine la test"),
            Grade.Create(classroom1.Id, student3.Id, teacher1.Id, 7, "Bine, mai e loc de îmbunătățire"),
            Grade.Create(classroom1.Id, student7.Id, teacher1.Id, 6, "Necesită exerciții suplimentare"),
            Grade.Create(classroom1.Id, student7.Id, teacher1.Id, 8, "Progres vizibil!"),
            // Classroom 2 — Informatică (teacher2)
            Grade.Create(classroom2.Id, student1.Id, teacher2.Id, 10, "Excelent la programare"),
            Grade.Create(classroom2.Id, student4.Id, teacher2.Id, 8, "Bine, continuă!"),
            Grade.Create(classroom2.Id, student4.Id, teacher2.Id, 9, "Foarte bine la test"),
            Grade.Create(classroom2.Id, student5.Id, teacher2.Id, 6, "Trebuie să exersezi mai mult"),
            Grade.Create(classroom2.Id, student8.Id, teacher2.Id, 10, "Perfect!"),
            Grade.Create(classroom2.Id, student8.Id, teacher2.Id, 9, "Foarte bine"),
            // Classroom 3 — Română (teacher3)
            Grade.Create(classroom3.Id, student2.Id, teacher3.Id, 9, "Excelent la gramatică"),
            Grade.Create(classroom3.Id, student3.Id, teacher3.Id, 7, "Bine, atenție la sintaxă"),
            Grade.Create(classroom3.Id, student6.Id, teacher3.Id, 10, "Foarte bine la eseu!"),
            Grade.Create(classroom3.Id, student6.Id, teacher3.Id, 9, "Bine la analiza sintactică"),
            Grade.Create(classroom3.Id, student7.Id, teacher3.Id, 8, "Progres bun"),
            Grade.Create(classroom3.Id, student8.Id, teacher3.Id, 7, "Bine, mai citește")
        );
        await db.SaveChangesAsync();
    }
}
