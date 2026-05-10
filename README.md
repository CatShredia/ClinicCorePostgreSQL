# 🏥 ClinicCore — Hospital Records Management System

A full-featured hospital records management desktop application built with **C#**, **Avalonia UI**, and **SQL Server**.

## ✨ Features
- 📊 **Dashboard** — Live stats (patients, doctors, appointments, prescriptions)
- 👤 **Patients** — Full CRUD management
- 🩺 **Doctors** — Manage doctor profiles and specialties
- 📅 **Appointments** — Schedule and track appointments
- 💊 **Prescriptions** — Issue and manage prescriptions
- 🗄️ **SQL Server** — Real database with Docker

## 🛠️ Tech Stack
| Layer | Technology |
|---|---|
| Language | C# (.NET 10) |
| UI Framework | Avalonia UI 12 |
| Database | SQL Server 2022 |
| Container | Docker |

## 🚀 Run Locally
```bash
# Start SQL Server
docker run -e ACCEPT_EULA=Y -e SA_PASSWORD=YourPassword123! -p 1433:1433 --name cliniccore-db -d mcr.microsoft.com/mssql/server:2022-latest

# Run app
cd ClinicCore
dotnet run
```

## 👩‍💻 Author
**Diya** — [@DIYA73](https://github.com/DIYA73)
