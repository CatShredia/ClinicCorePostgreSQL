-- ClinicCore test data seed (30+ rows per table)
-- Run after schema.sql: psql -h localhost -U postgres -d clinicdb -f database/seed.sql
-- File encoding: UTF-8, ASCII-safe strings (no Cyrillic) for Windows psql compatibility

SET client_encoding TO 'UTF8';

CREATE EXTENSION IF NOT EXISTS pgcrypto;

TRUNCATE TABLE "RefreshTokens", "Users", "Roles",
               "Prescriptions", "Appointments", "Doctors", "Patients"
RESTART IDENTITY CASCADE;

-- Roles (5)
INSERT INTO "Roles" ("RoleName", "Description") VALUES
('Admin',        'System administrator'),
('Doctor',       'Medical doctor'),
('Receptionist', 'Front desk staff'),
('Nurse',        'Nursing staff'),
('Manager',      'Clinic manager');

-- Users (30)
INSERT INTO "Users" ("Username", "PasswordHash", "FullName", "RoleID")
SELECT
    CASE i
        WHEN 1 THEN 'admin'
        WHEN 2 THEN 'doctor'
        WHEN 3 THEN 'reception'
        ELSE 'user' || LPAD(i::TEXT, 2, '0')
    END,
    crypt(
        CASE i
            WHEN 1 THEN 'admin123'
            WHEN 2 THEN 'doctor123'
            WHEN 3 THEN 'reception123'
            ELSE 'pass123'
        END,
        gen_salt('bf')
    ),
    CASE i
        WHEN 1 THEN 'System Administrator'
        WHEN 2 THEN 'Dr. Ivan Petrov'
        WHEN 3 THEN 'Anna Smirnova'
        ELSE 'Staff Member ' || i
    END,
    CASE i
        WHEN 1 THEN 1
        WHEN 2 THEN 2
        WHEN 3 THEN 3
        ELSE ((i - 1) % 5) + 1
    END
FROM generate_series(1, 30) AS i;

-- RefreshTokens (30) - linked to existing users
INSERT INTO "RefreshTokens" ("UserID", "Token", "ExpiresAt", "IsRevoked")
SELECT
    u."UserID",
    encode(gen_random_bytes(32), 'hex'),
    CURRENT_TIMESTAMP + (ROW_NUMBER() OVER (ORDER BY u."UserID") || ' days')::INTERVAL,
    u."UserID" % 7 = 0
FROM "Users" u
ORDER BY u."UserID"
LIMIT 30;

-- Patients (30)
INSERT INTO "Patients" ("FullName", "DateOfBirth", "Gender", "Phone", "Address")
SELECT
    (ARRAY[
        'Ivanov Ivan Ivanovich', 'Petrova Maria Sergeevna', 'Sidorov Alexey Petrovich',
        'Kozlova Elena Viktorovna', 'Novikov Dmitry Andreevich', 'Morozova Anna Igorevna',
        'Volkov Sergey Nikolaevich', 'Sokolova Olga Pavlovna', 'Lebedev Maxim Olegovich',
        'Kuznetsova Tatyana Romanovna', 'Popov Artyom Vladimirovich', 'Vasilieva Natalya Yuryevna',
        'Smirnov Pavel Gennadievich', 'Fedorova Ksenia Dmitrievna', 'Mikhailov Roman Stepanovich',
        'Orlova Victoria Alexandrovna', 'Andreev Kirill Borisovich', 'Nikolaeva Yulia Konstantinovna',
        'Egorov Vladislav Igorevich', 'Zakharova Svetlana Mikhailovna', 'Pavlov Georgy Anatolyevich',
        'Semenova Darya Evgenievna', 'Golubev Ilya Fedorovich', 'Vinogradova Alina Sergeevna',
        'Borisov Timofey Valerievich', 'Frolova Ekaterina Olegovna', 'Komarov Arseny Pavlovich',
        'Danilova Polina Andreevna', 'Zhukov Matvey Nikolaevich', 'Romanova Veronika Ilyinichna'
    ])[i],
    TO_CHAR(DATE '1960-01-01' + (i * 397 || ' days')::INTERVAL, 'YYYY-MM-DD'),
    CASE WHEN i % 2 = 0 THEN 'Female' ELSE 'Male' END,
    '+7 (9' || LPAD((10 + (i % 89))::TEXT, 2, '0') || ') ' ||
        LPAD((100 + i)::TEXT, 3, '0') || '-' ||
        LPAD((2000 + i * 37 % 9000)::TEXT, 4, '0'),
    'Moscow, Sample St. ' || i
FROM generate_series(1, 30) AS i;

-- Doctors (30)
INSERT INTO "Doctors" ("FullName", "Specialty", "Phone", "Email")
SELECT
    'Dr. ' || (ARRAY[
        'Alexeev V.P.', 'Belova N.S.', 'Gromov A.K.', 'Davydova L.M.', 'Ershov I.T.',
        'Zhuravlev O.R.', 'Zaitseva E.A.', 'Ivanchenko M.V.', 'Kalinina S.G.', 'Larionov P.D.',
        'Medvedeva T.I.', 'Nesterov K.L.', 'Osipova R.N.', 'Petrenko D.S.', 'Rybakov V.E.',
        'Savelyeva A.Yu.', 'Tikhonov G.M.', 'Uvarova O.V.', 'Fomin S.A.', 'Kharitonov N.P.',
        'Tsvetkova M.K.', 'Chernov A.B.', 'Shirokov I.G.', 'Shchukina E.L.', 'Yudin V.S.',
        'Yakovlev P.N.', 'Antonov R.D.', 'Baranova K.O.', 'Vlasov T.E.', 'Grigoriev L.Kh.'
    ])[i],
    (ARRAY[
        'Therapist', 'Cardiologist', 'Neurologist', 'Surgeon', 'Pediatrician',
        'Ophthalmologist', 'Dermatologist', 'Orthopedist', 'Gynecologist', 'Urologist',
        'Endocrinologist', 'Psychiatrist', 'Oncologist', 'ENT', 'Dentist',
        'Allergist', 'Gastroenterologist', 'Pulmonologist', 'Rheumatologist', 'Infectiologist',
        'Nephrologist', 'Hematologist', 'Physiotherapist', 'Anesthesiologist', 'Radiologist',
        'Pathologist', 'Family Doctor', 'Sports Medicine', 'Plastic Surgeon', 'Traumatologist'
    ])[i],
    '+7 (495) ' || LPAD((100 + i)::TEXT, 3, '0') || '-' || LPAD((1000 + i * 111 % 9000)::TEXT, 4, '0'),
    'doctor' || i || '@cliniccore.local'
FROM generate_series(1, 30) AS i;

-- Appointments (30)
INSERT INTO "Appointments" ("PatientID", "DoctorID", "AppointmentDate", "Status", "Notes")
SELECT
    ((i - 1) % 30) + 1,
    ((i * 7 - 1) % 30) + 1,
    TO_CHAR(CURRENT_DATE + (i || ' days')::INTERVAL, 'YYYY-MM-DD') || ' 10:00',
    (ARRAY['Scheduled', 'Completed', 'Cancelled', 'Rescheduled'])[1 + (i % 4)],
    CASE
        WHEN i % 3 = 0 THEN 'Initial visit'
        WHEN i % 3 = 1 THEN 'Follow-up exam'
        ELSE 'Lab results consultation'
    END
FROM generate_series(1, 30) AS i;

-- Prescriptions (30)
INSERT INTO "Prescriptions" ("PatientID", "DoctorID", "Medication", "Dosage", "Notes")
SELECT
    ((i * 3 - 1) % 30) + 1,
    ((i * 5 - 1) % 30) + 1,
    (ARRAY[
        'Paracetamol', 'Ibuprofen', 'Amoxicillin', 'Omeprazole', 'Loratadine',
        'Aspirin', 'Metformin', 'Amlodipine', 'Atorvastatin', 'Levothyroxine',
        'Cetirizine', 'Ambroxol', 'Nurofen', 'No-Spa', 'Analgin',
        'Validol', 'Corvalol', 'Furacilin', 'Miramistin', 'Acriol',
        'Diclofenac', 'Ketorol', 'Ciprofloxacin', 'Azithromycin', 'Fluconazole',
        'Pancreatin', 'Mezym', 'Suprastin', 'Tavegil', 'Enalapril'
    ])[i],
    (ARRAY[
        '500 mg 3 times daily', '200 mg twice daily', '250 mg 3 times daily',
        '20 mg once daily', '10 mg once daily', '100 mg once daily',
        '850 mg twice daily', '5 mg once daily', '10 mg at bedtime',
        '50 mcg in the morning', '10 mg once daily', '30 mg 3 times daily',
        '200 mg as needed', '40 mg twice daily', '500 mg as needed',
        '1 tablet sublingual', '15 drops 3 times daily', '1 tablet dissolved',
        '1 ml 3 times daily', 'as directed', '50 mg twice daily',
        '10 mg IM', '500 mg twice daily', '500 mg once daily',
        '150 mg once daily', '1 tablet with food', '1 tablet with food',
        '1 tablet at bedtime', '1 tablet twice daily', '5 mg once daily'
    ])[i],
    CASE
        WHEN i % 2 = 0 THEN 'Take after meals'
        ELSE '7-day course'
    END
FROM generate_series(1, 30) AS i;

-- Verify row counts
SELECT 'Roles' AS table_name, COUNT(*) AS row_count FROM "Roles"
UNION ALL SELECT 'Users', COUNT(*) FROM "Users"
UNION ALL SELECT 'RefreshTokens', COUNT(*) FROM "RefreshTokens"
UNION ALL SELECT 'Patients', COUNT(*) FROM "Patients"
UNION ALL SELECT 'Doctors', COUNT(*) FROM "Doctors"
UNION ALL SELECT 'Appointments', COUNT(*) FROM "Appointments"
UNION ALL SELECT 'Prescriptions', COUNT(*) FROM "Prescriptions";
