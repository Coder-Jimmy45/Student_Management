# 📘 Student Management API

## 🚀 Overview
This project is a Student Management System built using ASP.NET Core Web API.

It provides CRUD operations and secure access using JWT Authentication.

---

## 🛠️ Tech Stack
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger
- ILogger (Logging)

---

## 📂 Features
- Get all students
- Add student
- Update student
- Delete student
- JWT Authentication
- Exception Handling Middleware
- Layered Architecture

---

## ⚙️ Setup

1. Clone the repo
2. Open in Visual Studio
3. Update connection string in appsettings.json
4. Run:
   Add-Migration InitialCreate
   Update-Database
5. Run project
6. Open Swagger

---

## 🔐 Login

POST /api/auth/login

Note: Credentials and JWT secret are not stored in the repository. Configure them using dotnet user-secrets or environment variables before running.

Example (set with dotnet user-secrets in development):

```bash
dotnet user-secrets set "User:Username" "admin"
dotnet user-secrets set "User:Password" "<strong-password>"
dotnet user-secrets set "Jwt:Key" "<your-very-strong-key>"
```

Then call:

```json
{
  "username": "admin",
  "password": "<your-password>"
}
"}