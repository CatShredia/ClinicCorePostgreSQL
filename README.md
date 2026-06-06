# 🏥 ClinicCore — Hospital Records Management System

A full-featured hospital records management desktop application built with **C#**, **Avalonia UI**, and **PostgreSQL**.

## ✨ Features
- 📊 **Dashboard** — Live stats (patients, doctors, appointments, prescriptions)
- 👤 **Patients** — Full CRUD management
- 🩺 **Doctors** — Manage doctor profiles and specialties
- 📅 **Appointments** — Schedule and track appointments
- 💊 **Prescriptions** — Issue and manage prescriptions
- 🗄️ **PostgreSQL** — Local database

## 🛠️ Tech Stack
| Layer | Technology |
|---|---|
| Language | C# (.NET 10) |
| UI Framework | Avalonia UI 12 |
| Database | PostgreSQL |
| Driver | Npgsql |

## 🚀 Run Locally

### 1. Start PostgreSQL

Ensure PostgreSQL is running locally on port **5432** with database `postgres`.

```bash
# Docker (optional)
docker run -e POSTGRES_PASSWORD=postgres -p 5432:5432 --name cliniccore-pg -d postgres:16
```

Connection string (equivalent to `jdbc:postgresql://localhost:5432/postgres`):
```
Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres
```

Change the password in `ClinicCore/Database/DBHelper.cs` if your local setup differs.

### 2. Initialize database schema

```bash
psql -h localhost -U postgres -d postgres -f database/schema.sql
```

### 3. Seed test data (30 rows per table)

```bash
psql -h localhost -U postgres -d postgres -f database/seed.sql
```

### 4. Run the app

```bash
cd ClinicCore
dotnet run
```

## 📁 Database Scripts

| File | Description |
|---|---|
| `database/schema.sql` | Creates tables: Patients, Doctors, Appointments, Prescriptions |
| `database/seed.sql` | Inserts 30 test rows into each table |
