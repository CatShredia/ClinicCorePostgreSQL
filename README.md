# 🏥 ClinicCore — Hospital Records Management System

A full-featured hospital records management desktop application built with **C#**, **Avalonia UI**, and **PostgreSQL**.

## ✨ Features
- 🔐 **JWT Authentication** — Login with JWT access tokens and refresh tokens
- 📊 **Dashboard** — Live stats (patients, doctors, appointments, prescriptions)
- 👤 **Patients** — Full CRUD management
- 🩺 **Doctors** — Manage doctor profiles and specialties
- 📅 **Appointments** — Schedule and track appointments
- 💊 **Prescriptions** — Issue and manage prescriptions
- 🌐 **Localization** — Russian / English UI
- 🗄️ **PostgreSQL** — Local database

## 🛠️ Tech Stack
| Layer | Technology |
|---|---|
| Language | C# (.NET 10) |
| UI Framework | Avalonia UI 12 |
| Database | PostgreSQL + pgcrypto |
| Driver | Npgsql |
| Auth | JWT (System.IdentityModel.Tokens.Jwt) |

## 🚀 Run Locally

### 1. Start PostgreSQL

Ensure PostgreSQL is running locally on port **5432**.

### 2. Initialize database schema

```bash
psql -h localhost -U postgres -d clinicdb -f database/schema.sql
```

### 3. Seed test data (30 rows per table)

```bash
psql -h localhost -U postgres -d clinicdb -f database/seed.sql
```

### 4. Run the app

```bash
cd ClinicCore
dotnet run
```

## 🔐 Test Accounts

| Username | Password | Role |
|---|---|---|
| `admin` | `admin123` | Administrator |
| `doctor` | `doctor123` | Doctor |
| `reception` | `reception123` | Receptionist |
| `user04`–`user30` | `pass123` | Various roles |

## 📁 Database Scripts

| File | Description |
|---|---|
| `database/schema.sql` | Creates Roles, Users, RefreshTokens + clinical tables |
| `database/seed.sql` | Inserts 30 test rows into each table |

## 📋 Tables

| Table | Purpose |
|---|---|
| `Roles` | User roles (Admin, Doctor, etc.) |
| `Users` | Accounts with bcrypt password hashes |
| `RefreshTokens` | JWT refresh tokens |
| `Patients` | Patient records |
| `Doctors` | Doctor profiles |
| `Appointments` | Scheduled visits |
| `Prescriptions` | Issued medications |
