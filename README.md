# Student Management API

A portfolio-oriented .NET 8 Web API for managing student records. It uses SQL Server, EF Core migrations, JWT bearer authentication, validation, structured API errors, and a layered controller/service/repository design.

## Features

- Register and authenticate users with salted, one-way password hashes.
- JWT-protected student CRUD endpoints.
- Request validation and RFC 7807-style error responses.
- Unique student email, database age constraint, bounded database columns, UTC timestamps, and optimistic-concurrency support.
- Paginated student listings and Swagger in the Development environment.

## Architecture

```text
HTTP controllers -> services -> repositories -> EF Core -> SQL Server
```

Controllers expose DTOs only. Services hold business rules and repositories own persistence queries.

## Secure local configuration

No credentials, connection strings, or JWT keys are stored in this repository. Copy the shape from `Student_Management/appsettings.example.json`, then set local user secrets:

```powershell
cd Student_Management
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=StudentManagementDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=False;"
dotnet user-secrets set "Jwt:Key" "generate-a-unique-random-key-of-at-least-32-bytes"
dotnet user-secrets set "Jwt:Issuer" "StudentManagement.Api"
dotnet user-secrets set "Jwt:Audience" "StudentManagement.Client"
```

Use a secrets manager or deployment platform configuration in non-local environments. Never commit live settings, credentials, or JWT signing keys.

## Database

Apply the EF Core migrations after configuration:

```powershell
dotnet ef database update
```

The hardening migration adds the `Users` table, enforces a unique student email, applies field limits, adds an age constraint, and adds a row-version column. Existing duplicate or invalid student data must be corrected before applying it.

## Run

```powershell
dotnet run --project Student_Management
```

In Development, Swagger is available at `/swagger` over HTTPS. Start by calling `POST /api/auth/register`, then call `POST /api/auth/login` and supply the returned token as `Authorization: Bearer <token>`.

## Endpoints

| Method | Route | Authentication | Purpose |
| --- | --- | --- | --- |
| POST | `/api/auth/register` | Public | Create a user with a hashed password |
| POST | `/api/auth/login` | Public | Receive a 60-minute JWT |
| GET | `/api/students?page=1&pageSize=20` | Bearer token | List students |
| GET | `/api/students/{id}` | Bearer token | Fetch a student |
| POST | `/api/students` | Bearer token | Create a student |
| PUT | `/api/students/{id}` | Bearer token | Update a student |
| DELETE | `/api/students/{id}` | Bearer token | Delete a student |

## Verification and tests

```powershell
dotnet build Student_Management.sln
dotnet test Student_Management.sln
```

The solution includes unit tests for student-service business rules. Add HTTP integration tests and a CI workflow before public deployment.

## Security note

Earlier commits contain exposed development credentials and a JWT key. Rotate them and, if the repository has been published, rewrite the remote Git history using a reviewed secret-removal procedure before presenting the repository publicly.
