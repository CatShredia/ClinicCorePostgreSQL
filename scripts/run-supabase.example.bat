@echo off
REM Copy this file to run-supabase.bat and fill in your Supabase credentials.
REM Supabase: Project Settings -> Database -> Connection info (Direct connection)

set CLINICCORE_DB_HOST=db.YOUR_PROJECT_REF.supabase.co
set CLINICCORE_DB_PORT=5432
set CLINICCORE_DB_NAME=postgres
set CLINICCORE_DB_USER=postgres
set CLINICCORE_DB_PASSWORD=YOUR_DATABASE_PASSWORD

start "" "%~dp0ClinicCore.exe"
