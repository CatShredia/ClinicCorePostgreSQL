-- ClinicCore PostgreSQL schema
-- Requires: CREATE EXTENSION pgcrypto (for password hashing)

CREATE EXTENSION IF NOT EXISTS pgcrypto;

DROP TABLE IF EXISTS "RefreshTokens" CASCADE;
DROP TABLE IF EXISTS "Users" CASCADE;
DROP TABLE IF EXISTS "Roles" CASCADE;
DROP TABLE IF EXISTS "Prescriptions" CASCADE;
DROP TABLE IF EXISTS "Appointments" CASCADE;
DROP TABLE IF EXISTS "Doctors" CASCADE;
DROP TABLE IF EXISTS "Patients" CASCADE;

CREATE TABLE "Roles" (
    "RoleID"      SERIAL PRIMARY KEY,
    "RoleName"    TEXT NOT NULL UNIQUE,
    "Description" TEXT
);

CREATE TABLE "Users" (
    "UserID"       SERIAL PRIMARY KEY,
    "Username"     TEXT NOT NULL UNIQUE,
    "PasswordHash" TEXT NOT NULL,
    "FullName"     TEXT NOT NULL,
    "RoleID"       INT NOT NULL REFERENCES "Roles"("RoleID"),
    "IsActive"     BOOLEAN NOT NULL DEFAULT TRUE,
    "LastLoginAt"  TIMESTAMP,
    "CreatedAt"    TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "RefreshTokens" (
    "RefreshTokenID" SERIAL PRIMARY KEY,
    "UserID"         INT NOT NULL REFERENCES "Users"("UserID") ON DELETE CASCADE,
    "Token"          TEXT NOT NULL UNIQUE,
    "ExpiresAt"      TIMESTAMP NOT NULL,
    "IsRevoked"      BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt"      TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "Patients" (
    "PatientID"   SERIAL PRIMARY KEY,
    "FullName"    TEXT NOT NULL,
    "DateOfBirth" TEXT,
    "Gender"      TEXT,
    "Phone"       TEXT,
    "Address"     TEXT,
    "CreatedAt"   TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "Doctors" (
    "DoctorID"  SERIAL PRIMARY KEY,
    "FullName"  TEXT NOT NULL,
    "Specialty" TEXT,
    "Phone"     TEXT,
    "Email"     TEXT
);

CREATE TABLE "Appointments" (
    "AppointmentID"   SERIAL PRIMARY KEY,
    "PatientID"       INT REFERENCES "Patients"("PatientID"),
    "DoctorID"        INT REFERENCES "Doctors"("DoctorID"),
    "AppointmentDate" TEXT,
    "Status"          TEXT,
    "Notes"           TEXT
);

CREATE TABLE "Prescriptions" (
    "PrescriptionID" SERIAL PRIMARY KEY,
    "PatientID"      INT REFERENCES "Patients"("PatientID"),
    "DoctorID"       INT REFERENCES "Doctors"("DoctorID"),
    "Medication"     TEXT,
    "Dosage"         TEXT,
    "IssuedDate"     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "Notes"          TEXT
);

CREATE INDEX "IX_Users_Username" ON "Users"("Username");
CREATE INDEX "IX_RefreshTokens_Token" ON "RefreshTokens"("Token");
CREATE INDEX "IX_RefreshTokens_UserID" ON "RefreshTokens"("UserID");
