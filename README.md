# AI Tutor

An educational platform that uses AI to help Romanian primary and middle school students learn at their own pace.

## About

AI Tutor started from a simple observation: kids learn at very different speeds, and a textbook can't adapt to each of them. The platform covers Mathematics, Romanian language, and Computer Science for primary and middle school students, and the part that makes it different from a regular e-learning site is the AI tutor itself. Using Google Gemini, the app generates explanations tailored to the student's age and level, so the same topic, fractions for instance, gets explained one way to a 4th grader and a completely different way to an 8th grader.

The platform is built around three user roles, each with its own dashboard and workflow:

- **Students** go through lessons, take quizzes, ask for AI-generated explanations when something doesn't click, and track their own progress.
- **Parents** see how their children are doing across subjects, check grades, and manage the subscription.
- **Teachers** create virtual classrooms, add lessons, and monitor how their students are performing.

## What it does

- JWT authentication with role-based access (Student, Parent, Teacher, Admin), passwords hashed with BCrypt
- Structured lessons organized by subject, grade, and difficulty
- Interactive quizzes with multiple-choice questions and instant feedback
- AI-generated explanations adapted to the student's age, powered by Google Gemini
- Progress tracking per student and per subject
- Parent-child relationships with an aggregated view across all enrolled children
- Recurring subscriptions through Stripe, monthly or yearly, for access to premium content
- Virtual classrooms created by teachers with student enrollment

## Architecture

The backend follows Clean Architecture with strict separation of concerns and CQRS implemented through MediatR. The frontend is a Blazor WebAssembly SPA, and the AI module is a separate Python service that the backend calls over HTTP.

```
┌─────────────────────────────────────────────────────────┐
│                   AiTutor.API                           │
│  Controllers, middleware, authentication                │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│              AiTutor.Application                        │
│  Commands, queries, handlers, validators, DTOs          │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│                AiTutor.Domain                           │
│  Entities, value objects, domain logic, enums           │
└──────────────────────▲──────────────────────────────────┘
                       │
┌──────────────────────┴──────────────────────────────────┐
│             AiTutor.Infrastructure                      │
│  EF Core, PostgreSQL, Stripe, JWT, external APIs        │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│           AI module (Python, FastAPI)                   │
│        Google Gemini integration, REST API              │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│         AiTutor.Web (Blazor WebAssembly)                │
│      Components, Fluxor state, API client               │
└─────────────────────────────────────────────────────────┘
```

## Tech stack

**Backend.** .NET 10 with ASP.NET Core Web API. The application layer uses MediatR for CQRS, FluentValidation for input validation, AutoMapper for entity-to-DTO mappings, and Entity Framework Core as the ORM. Passwords are hashed with BCrypt.Net.

**Database.** PostgreSQL 16. It runs in a Docker container locally and as a managed service in production. Migrations are versioned in code through EF Core.

**Frontend.** Blazor WebAssembly, with Fluxor for state management (a Redux-style pattern adapted to Blazor).

**AI module.** A separate Python 3.12 service built with FastAPI. It talks to the .NET backend over REST and uses the Google Gemini API (`gemini-2.0-flash`) to generate explanations.

**Payments.** Stripe, using checkout sessions for the subscription flow and webhooks to keep the subscription status in sync.

**Testing.** xUnit for unit tests, Moq for mocking, Coverlet for coverage reports in OpenCover format. A separate integration test project runs against a real PostgreSQL instance.

**Code quality.** SonarQube locally during development and SonarCloud in CI. Static analysis covers reliability, security, and maintainability.

**Containerization.** Docker and Docker Compose orchestrate the four services (postgres, backend, ai-module, frontend) in local development.

**CI/CD.** GitHub Actions. Every push to `main` triggers a full pipeline: build, tests with coverage, SonarCloud analysis, Docker image build, push to Azure Container Registry, and deployment to Azure Container Apps.

**Cloud.** Microsoft Azure: Container Apps for the runtime, Container Registry for the images, and Container App secrets for sensitive values like the Gemini API key, database connection string, and Stripe keys.

## Code quality at a glance

| Metric | Value |
|---|---|
| Test coverage | 88% |
| Unit tests | 339 passing |
| Quality Gate | Passed |
| Reliability rating | A |
| Security rating | A |
| Maintainability rating | A |
| Bugs | 0 |
| Vulnerabilities | 0 |

## Getting started locally

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 10 SDK](https://dotnet.microsoft.com/download) (only if you want to run things outside Docker)
- A Google Gemini API key from [Google AI Studio](https://aistudio.google.com/app/apikey)

### Steps

Clone the repository:

```bash
git clone https://github.com/RazvanMV1/ai-tutor.git
cd ai-tutor
```

Create a `.env` file in the project root with your own values:

```env
GEMINI_API_KEY=your_gemini_key_here
GOOGLE_API_KEY=your_gemini_key_here
POSTGRES_PASSWORD=your_password_here
JWT_SECRET=a_long_random_string_at_least_32_chars
STRIPE_SECRET_KEY=sk_test_...
STRIPE_WEBHOOK_SECRET=whsec_...
```

Bring up the full stack:

```bash
docker-compose up -d
```

Check that everything is running:

```bash
docker ps
```

Then open:

- Frontend: http://localhost:3000
- Backend API (Swagger): http://localhost:5000/swagger
- AI module (Swagger): http://localhost:8000/docs
- PostgreSQL: `localhost:5432`

### Test accounts

The database is seeded on first run with the following accounts:

| Role | Email | Password |
|---|---|---|
| Admin | `admin@aitutor.com` | `Admin123!` |
| Parent | `parent@aitutor.com` | `Parent123!` |
| Student | `student@aitutor.com` | `Student123!` |
| Teacher | `teacher@aitutor.com` | `Teacher123!` |

## Running the tests

Run all unit tests:

```bash
dotnet test tests/AiTutor.UnitTests/AiTutor.UnitTests.csproj
```

Run the full coverage pipeline (builds, tests, and publishes to a local SonarQube instance):

```powershell
.\scripts\run-coverage.ps1
```

The report will be available at http://localhost:9000/dashboard?id=AiTutor.

## Project structure

```
ai-tutor/
├── .github/workflows/         GitHub Actions CI/CD
├── scripts/                   PowerShell utility scripts
├── src/
│   ├── backend/
│   │   ├── AiTutor.API/              Web API entry point
│   │   ├── AiTutor.Application/      CQRS handlers, DTOs, validators
│   │   ├── AiTutor.Domain/           Entities, value objects, enums
│   │   └── AiTutor.Infrastructure/   EF Core, external services
│   ├── frontend/
│   │   └── AiTutor.Web/              Blazor WebAssembly
│   └── ai-module/                    Python FastAPI + Gemini
├── tests/
│   ├── AiTutor.UnitTests/            Unit tests (xUnit, Moq)
│   └── AiTutor.IntegrationTests/     Integration tests with Postgres
├── docker-compose.yml
└── README.md
```

## CI/CD pipeline

Every push to `main` runs the following steps in GitHub Actions:

1. Checkout the code
2. Set up .NET 10 and Java 17 (the latter is needed by SonarScanner)
3. Restore dependencies
4. Begin SonarCloud analysis
5. Build the solution
6. Run tests with coverage collection (Coverlet, OpenCover format)
7. End SonarCloud analysis and publish results
8. Build and push the Docker images to Azure Container Registry
9. Deploy the new revision to Azure Container Apps

## Deployment

The app is deployed on Microsoft Azure using:

- **Azure Container Apps** for the backend and the AI module, running as serverless containers
- **Azure Container Registry** for the Docker images
- **Azure Database for PostgreSQL** as the managed database
- **Container App secrets** for API keys and connection strings

## Contributing

Contributions are welcome. For larger changes, please open an issue first so we can talk about what you have in mind.

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -m "Add your feature"`)
4. Push the branch (`git push origin feature/your-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Author

Built by [Răzvan](https://github.com/RazvanMV1) as part of a university .NET course.

## Acknowledgements

Thanks to Google for the Gemini API, to the .NET community for an outstanding ecosystem, to Stripe for their developer experience, and to SonarSource for the code quality tooling.
