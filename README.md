# 🏥 ClinicCore — Hospital Records Management System

A full-featured hospital records management desktop application built with **C#**, **Avalonia UI**, and **PostgreSQL** (local or **Supabase**).

## ✨ Features
- 🔐 **JWT Authentication** — Login with JWT access tokens and refresh tokens
- 👥 **Role-based access** — Admin, Manager, Doctor, Receptionist, Nurse
- 📊 **Dashboard** — Live stats (patients, doctors, appointments, prescriptions)
- 👤 **Patients / Doctors / Appointments / Prescriptions** — CRUD
- 🛡️ **User management** — Admin panel with access matrix
- 🌐 **Localization** — Russian / English UI

## 🛠️ Tech Stack
| Layer | Technology |
|---|---|
| Language | C# (.NET 10) |
| UI Framework | Avalonia UI 12 |
| Database | PostgreSQL (Supabase or local) |
| Driver | Npgsql |
| Auth | JWT |

---

## 📦 Try the demo (GitHub Releases + Supabase)

Best way for users to explore the app **without cloning the repo**.

### Step 1 — Create a free Supabase project

1. Go to [supabase.com](https://supabase.com) and create a project.
2. Open **SQL Editor** and run scripts in order:
   - `database/schema.sql`
   - `database/seed.sql`
3. Open **Project Settings → Database** and copy:
   - **Host** (`db.xxxxx.supabase.co`)
   - **Database password**

### Step 2 — Download the app

1. Open **[GitHub Releases](https://github.com/YOUR_OWNER/ClinicCorePostgreSQL/releases)** (replace with your repo URL).
2. Download `ClinicCore-win-x64.zip` or `ClinicCore.exe`.
3. Unzip to a folder.

### Step 3 — Connect to Supabase and run

1. Copy `run-supabase.example.bat` → `run-supabase.bat`.
2. Fill in your Supabase host and password:

```bat
set CLINICCORE_DB_HOST=db.xxxxx.supabase.co
set CLINICCORE_DB_PASSWORD=your-database-password
```

3. Double-click `run-supabase.bat`.

### Step 4 — Log in

| Username | Password | Role |
|---|---|---|
| `admin` | `admin123` | Administrator |
| `doctor` | `doctor123` | Doctor |
| `reception` | `reception123` | Receptionist |
| `user04`–`user30` | `pass123` | Various roles |

---

## 🔧 Connection configuration

The app reads database settings from **environment variables** (no secrets in source code).

| Variable | Description |
|---|---|
| `CLINICCORE_CONNECTION_STRING` | Full Npgsql connection string (overrides all below) |
| `CLINICCORE_DB_HOST` | Database host (e.g. `db.xxx.supabase.co`) |
| `CLINICCORE_DB_PORT` | Port (default `5432`) |
| `CLINICCORE_DB_NAME` | Database name (default `postgres` for Supabase) |
| `CLINICCORE_DB_USER` | User (default `postgres`) |
| `CLINICCORE_DB_PASSWORD` | Database password |

See `supabase.env.example` for a template.

**Supabase connection string example:**

```
Host=db.xxxxx.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=SECRET;SSL Mode=Require;Trust Server Certificate=true
```

If no env vars are set, the app falls back to local `localhost:5432 / clinicdb`.

---

## 🚀 Run from source (local PostgreSQL)

```bash
psql -h localhost -U postgres -d clinicdb -f database/schema.sql
psql -h localhost -U postgres -d clinicdb -f database/seed.sql
cd ClinicCore
dotnet run
```

---

## 📤 Publish a new GitHub Release

Releases are built automatically by GitHub Actions when you push a version tag:

```bash
git tag v1.0.0
git push origin v1.0.0
```

Or trigger manually: **Actions → Release → Run workflow**.

The workflow publishes:
- `ClinicCore.exe` (Windows x64, self-contained)
- `ClinicCore-win-x64.zip` (exe + SQL scripts + `run-supabase.example.bat`)

---

## 📁 Database Scripts

| File | Description |
|---|---|
| `database/schema.sql` | Tables: Roles, Users, RefreshTokens, Patients, Doctors, … |
| `database/seed.sql` | 30 test rows per table |

## 📋 Role access matrix

| Section | Admin | Manager | Doctor | Receptionist | Nurse |
|---|---|---|---|---|---|
| Dashboard | ✓ | ✓ | ✓ | ✓ | ✓ |
| Patients | full | full | read | full | read |
| Doctors | full | full | read | read | read |
| Appointments | full | full | full | full | read |
| Prescriptions | full | full | full | — | read |
| Users | full | — | — | — | — |
