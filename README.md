# AiTutor - Platforma Educationala Inteligenta

Platforma moderna de e-learning cu tutor AI integrat, clase virtuale, quiz-uri interactive si evaluare automata.

---

## Cuprins

- Descriere
- Caracteristici principale
- Arhitectura
- Stack tehnologic
- Structura proiectului
- Cerinte de sistem
- Configurare si rulare
- Variabile de mediu
- Endpoint-uri API principale
- Roluri utilizatori
- Fluxul aplicatiei
- Tutor AI
- Sistem de plati
- Testare
- Roadmap
- Autor

---

## Descriere

AiTutor este o platforma educationala SaaS care combina functionalitatile unei scoli virtuale cu inteligenta artificiala.

Aplicatia permite:

- creare clase virtuale
- lectii organizate
- quiz-uri interactive
- evaluare automata
- monitorizare progres
- tutor AI bazat pe continutul lectiei

Roluri:

- Student
- Parent
- Teacher
- Admin

---

## Caracteristici principale

### Pentru elevi

- inscriere in clase prin cod
- lectii organizate
- quiz-uri cu timer
- trimitere automata la expirare
- feedback instant
- chat cu AI pe baza lectiei
- progres automat

### Pentru profesori

- creare clase
- gestionare elevi
- CRUD lectii
- creare quiz-uri
- creare intrebari
- catalog note
- progres elevi
- testare AI

### Pentru parinti

- conectare copil prin cod
- vizualizare note
- progres copil
- abonamente

### Functionalitati generale

- autentificare JWT
- plati Stripe
- webhook Stripe
- validare backend/frontend
- Docker Compose

---

## Arhitectura

Flux general:

Browser utilizator
-> Frontend Blazor WebAssembly
-> Backend ASP.NET Core API
-> PostgreSQL

Backend comunica si cu:

AI Module FastAPI
-> Google Gemini API

---

## Pattern-uri folosite

- Clean Architecture
- Domain Driven Design
- CQRS
- MediatR
- Repository Pattern
- Result Pattern
- Proxy Pattern

---

## Stack tehnologic

Backend:

- .NET 10
- ASP.NET Core
- Entity Framework Core
- MediatR
- FluentValidation
- JWT
- BCrypt
- Stripe.NET

Frontend:

- Blazor WebAssembly
- MudBlazor
- Nginx

AI Module:

- Python 3.11
- FastAPI
- Google Generative AI
- Pydantic
- Uvicorn

Infrastructura:

- PostgreSQL 16
- Docker Compose
- Stripe
- Gemini API

---

## Structura proiectului

ai-tutor/
docker-compose.yml
.env
README.md

src/
backend/
AiTutor.API/
AiTutor.Application/
AiTutor.Domain/
AiTutor.Infrastructure/

frontend/
AiTutor.Web/

ai-module/
app/
routers/
services/
schemas/
tests/

docs/
screenshots/

---

## Cerinte de sistem

- Docker Desktop 4.20+
- Docker Compose 2+
- Git
- minim 8GB RAM
- cont Google AI Studio
- cont Stripe test

Optional:

- .NET 10 SDK
- Python 3.11
- PostgreSQL

---

## Configurare si rulare

1. Clone repository:

git clone https://github.com/user/ai-tutor.git
cd ai-tutor

2. Creeaza fisier .env

3. Ruleaza:

docker-compose up -d --build

4. Verifica:

docker ps

5. Acces:

Frontend: http://localhost:3000
Backend: http://localhost:5000
Swagger: http://localhost:5000/swagger
AI: http://localhost:8000

6. Oprire:

docker-compose down

Reset DB:

docker-compose down -v

---

## Variabile de mediu

POSTGRES_DB=aitutor
POSTGRES_USER=aitutor_admin
POSTGRES_PASSWORD=parola

CONNECTION_STRING=Host=postgres;Port=5432;Database=aitutor;Username=aitutor_admin;Password=parola

JWT_SECRET_KEY=secret_min_32_chars
JWT_ISSUER=AiTutor
JWT_AUDIENCE=AiTutorUsers

GEMINI_API_KEY=key

STRIPE_SECRET_KEY=sk_test_key
STRIPE_WEBHOOK_SECRET=whsec_key

---

## Endpoint-uri API principale

Auth:

POST /api/Auth/register
POST /api/Auth/login

Clase:

GET /api/Classrooms
POST /api/Classrooms
POST /api/Classrooms/join

Lectii:

GET /api/Classrooms/{id}/lessons
POST /api/Classrooms/{id}/lessons

Quiz-uri:

POST /api/Classrooms/{id}/quizzes
POST /api/Classrooms/{id}/submit

AI:

POST /api/Ai/lesson-chat
POST /api/Ai/explanation
POST /api/Ai/hint

Abonamente:

POST /api/Subscriptions/checkout-session
GET /api/Subscriptions

---

## Roluri utilizatori

Student:
- participa la lectii
- face quiz-uri
- foloseste AI

Teacher:
- creeaza clase
- creeaza lectii
- creeaza quiz-uri
- noteaza elevi

Parent:
- vede progres copil
- plateste abonament

Admin:
- control total

---

## Fluxul aplicatiei

Profesor:
creeaza clasa
adauga lectii
adauga quiz-uri

Elev:
intra in clasa
parcurge lectii
face quiz
foloseste AI

Parinte:
se conecteaza
vede progres

---

## Tutor AI

Flux:

Frontend -> Backend -> FastAPI -> Gemini API

AI primeste:

- lectie
- intrebare
- istoric

AI raspunde contextual.

---

## Sistem de plati

Stripe Checkout

Tipuri:

Free
Parent Monthly
Parent Yearly
School

Card test:

4242 4242 4242 4242

---

## Testare

Backend:

dotnet test

AI:

pytest

---

## Roadmap

Implementat:

- autentificare
- clase
- lectii
- quiz-uri
- AI tutor
- Stripe
- Docker

In plan:

- generare quiz AI
- dashboard
- dark mode
- export PDF
- notificari
- deploy cloud

---

## Autor

Proiect academic.

Pentru contributii, foloseste GitHub issues.
