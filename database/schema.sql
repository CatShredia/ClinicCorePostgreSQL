-- ClinicCore PostgreSQL schema
-- Connection: jdbc:postgresql://localhost:5432/postgres

DROP TABLE IF EXISTS "Prescriptions" CASCADE;
DROP TABLE IF EXISTS "Appointments" CASCADE;
DROP TABLE IF EXISTS "Doctors" CASCADE;
DROP TABLE IF EXISTS "Patients" CASCADE;

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
