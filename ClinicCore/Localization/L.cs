using System.Collections.Generic;

namespace ClinicCore.Localization;

public static class L
{
    private static readonly Dictionary<string, string[]> Strings = new()
    {
        ["WindowTitle"]           = ["ClinicCore — Hospital Records", "ClinicCore — Медицинские записи"],
        ["Subtitle"]              = ["Hospital Records System", "Система учёта пациентов"],
        ["Menu"]                  = ["MENU", "МЕНЮ"],
        ["Language"]              = ["Language", "Язык"],
        ["NavDashboard"]          = ["Dashboard", "Панель"],
        ["NavPatients"]           = ["Patients", "Пациенты"],
        ["NavDoctors"]            = ["Doctors", "Врачи"],
        ["NavAppointments"]       = ["Appointments", "Записи"],
        ["NavPrescriptions"]      = ["Prescriptions", "Рецепты"],
        ["NavUsers"]              = ["Users", "Пользователи"],

        ["DashboardTitle"]        = ["Dashboard", "Панель управления"],
        ["Welcome"]               = ["Welcome to ClinicCore Hospital Records System", "Добро пожаловать в систему ClinicCore"],
        ["Patients"]              = ["Patients", "Пациенты"],
        ["Doctors"]               = ["Doctors", "Врачи"],
        ["Appointments"]          = ["Appointments", "Записи"],
        ["Prescriptions"]         = ["Prescriptions", "Рецепты"],
        ["TotalRegistered"]       = ["Total registered", "Всего зарегистрировано"],
        ["TotalScheduled"]        = ["Total scheduled", "Всего запланировано"],
        ["TotalIssued"]           = ["Total issued", "Всего выписано"],
        ["SystemInfo"]            = ["System Info", "Информация о системе"],
        ["Loading"]               = ["Loading...", "Загрузка..."],
        ["Connected"]             = ["Connected", "Подключено"],
        ["DbNotConnected"]        = ["Database not connected", "Нет подключения к базе данных"],
        ["DbInfo"]                = ["Database: {0}  |  Server: {1}  |  Status: {2}", "База данных: {0}  |  Сервер: {1}  |  Статус: {2}"],

        ["PatientsCount"]         = ["{0} patient(s) registered", "{0} пациент(ов) зарегистрировано"],
        ["DoctorsCount"]          = ["{0} doctor(s) registered", "{0} врач(ей) зарегистрировано"],
        ["ApptsCount"]            = ["{0} appointment(s) scheduled", "{0} записей запланировано"],
        ["RxCount"]               = ["{0} prescription(s) issued", "{0} рецептов выписано"],
        ["DbOffline"]             = ["DB not connected — showing offline mode", "Нет подключения — автономный режим"],
        ["DbNotConnectedShort"]   = ["DB not connected", "Нет подключения к БД"],

        ["AddPatient"]            = ["+ Add Patient", "+ Добавить пациента"],
        ["AddDoctor"]             = ["+ Add Doctor", "+ Добавить врача"],
        ["AddAppointment"]        = ["+ Add Appointment", "+ Добавить запись"],
        ["AddPrescription"]       = ["+ Add Prescription", "+ Добавить рецепт"],
        ["FormAddPatient"]        = ["Add New Patient", "Новый пациент"],
        ["FormEditPatient"]       = ["Edit Patient", "Редактировать пациента"],
        ["FormAddDoctor"]         = ["Add New Doctor", "Новый врач"],
        ["FormEditDoctor"]        = ["Edit Doctor", "Редактировать врача"],
        ["FormAddAppointment"]    = ["Add New Appointment", "Новая запись"],
        ["FormEditAppointment"]   = ["Edit Appointment", "Редактировать запись"],
        ["FormAddPrescription"]   = ["Add New Prescription", "Новый рецепт"],
        ["FormEditPrescription"]  = ["Edit Prescription", "Редактировать рецепт"],

        ["FullName"]              = ["Full Name", "ФИО"],
        ["DateOfBirth"]           = ["Date of Birth", "Дата рождения"],
        ["Gender"]                = ["Gender", "Пол"],
        ["Phone"]                 = ["Phone", "Телефон"],
        ["Address"]               = ["Address", "Адрес"],
        ["Specialty"]             = ["Specialty", "Специальность"],
        ["Email"]                 = ["Email", "Эл. почта"],
        ["PatientId"]             = ["Patient ID", "ID пациента"],
        ["DoctorId"]              = ["Doctor ID", "ID врача"],
        ["Date"]                  = ["Date", "Дата"],
        ["DateTime"]              = ["Date (YYYY-MM-DD HH:MM)", "Дата (ГГГГ-ММ-ДД ЧЧ:ММ)"],
        ["Status"]                = ["Status", "Статус"],
        ["Notes"]                 = ["Notes", "Примечания"],
        ["Medication"]            = ["Medication", "Препарат"],
        ["Dosage"]                = ["Dosage", "Дозировка"],
        ["IssuedDate"]            = ["Date", "Дата выписки"],

        ["PlaceholderFullName"]   = ["Full name", "ФИО"],
        ["PlaceholderDOB"]        = ["YYYY-MM-DD", "ГГГГ-ММ-ДД"],
        ["PlaceholderPhone"]      = ["Phone number", "Номер телефона"],
        ["PlaceholderAddress"]    = ["Address", "Адрес"],
        ["PlaceholderDoctorName"] = ["Doctor full name", "ФИО врача"],
        ["PlaceholderSpecialty"]  = ["e.g. Cardiology", "напр. Кардиология"],
        ["PlaceholderEmail"]      = ["Email address", "Адрес эл. почты"],
        ["PlaceholderPatientId"]  = ["Patient ID", "ID пациента"],
        ["PlaceholderDoctorId"]   = ["Doctor ID", "ID врача"],
        ["PlaceholderDateTime"]   = ["2025-01-15 10:00", "2025-01-15 10:00"],
        ["PlaceholderNotes"]      = ["Optional notes", "Необязательные примечания"],
        ["PlaceholderMedication"] = ["e.g. Paracetamol", "напр. Парацетамол"],
        ["PlaceholderDosage"]     = ["e.g. 500mg twice daily", "напр. 500 мг 2 раза в день"],

        ["Male"]                  = ["Male", "Мужской"],
        ["Female"]                = ["Female", "Женский"],
        ["Cancel"]                = ["Cancel", "Отмена"],
        ["SavePatient"]           = ["Save Patient", "Сохранить"],
        ["SaveDoctor"]            = ["Save Doctor", "Сохранить"],
        ["SaveAppointment"]       = ["Save Appointment", "Сохранить"],
        ["SavePrescription"]      = ["Save Prescription", "Сохранить"],
        ["EditSelected"]          = ["Edit Selected", "Редактировать"],
        ["DeleteSelected"]        = ["Delete Selected", "Удалить"],
        ["Error"]                 = ["Error", "Ошибка"],

        ["ColId"]                 = ["ID", "ID"],

        ["StatusScheduled"]       = ["Scheduled", "Запланировано"],
        ["StatusCompleted"]       = ["Completed", "Завершено"],
        ["StatusCancelled"]       = ["Cancelled", "Отменено"],
        ["StatusRescheduled"]     = ["Rescheduled", "Перенесено"],

        ["LoginTitle"]            = ["ClinicCore — Login", "ClinicCore — Вход"],
        ["LoginSubtitle"]         = ["Sign in to continue", "Вход в систему"],
        ["Username"]              = ["Username", "Логин"],
        ["Password"]              = ["Password", "Пароль"],
        ["PlaceholderUsername"]   = ["admin", "admin"],
        ["PlaceholderPassword"]   = ["••••••••", "••••••••"],
        ["Login"]                 = ["Sign In", "Войти"],
        ["LoginFailed"]           = ["Invalid username or password", "Неверный логин или пароль"],
        ["TestAccounts"]          = ["Test accounts:", "Тестовые учётные записи:"],
        ["TestAccountAdmin"]      = ["admin / admin123", "admin / admin123"],
        ["TestAccountDoctor"]     = ["doctor / doctor123", "doctor / doctor123"],
        ["TestAccountReception"]  = ["reception / reception123", "reception / reception123"],
        ["TestAccountStaff"]      = ["user04..user30 / pass123", "user04..user30 / pass123"],

        ["AccessMatrixTitle"]     = ["Access rights for selected role:", "Права доступа для выбранной роли:"],
        ["AccessFull"]            = ["full access", "полный доступ"],
        ["AccessRead"]            = ["read only", "только просмотр"],
        ["AccessNone"]            = ["no access", "нет доступа"],
        ["Logout"]                = ["Logout", "Выйти"],
        ["UserRole"]              = ["Role: {0}", "Роль: {0}"],
        ["AuthInfo"]              = ["User: {0} ({1})  |  JWT: active", "Пользователь: {0} ({1})  |  JWT: активен"],

        ["RoleAdmin"]             = ["Administrator", "Администратор"],
        ["RoleDoctor"]            = ["Doctor", "Врач"],
        ["RoleReceptionist"]      = ["Receptionist", "Регистратор"],
        ["RoleNurse"]             = ["Nurse", "Медсестра"],
        ["RoleManager"]           = ["Manager", "Менеджер"],

        ["Users"]                 = ["Users", "Пользователи"],
        ["UsersCount"]            = ["{0} user(s) registered", "{0} пользователей"],
        ["AddUser"]               = ["+ Add User", "+ Добавить пользователя"],
        ["FormAddUser"]           = ["Add New User", "Новый пользователь"],
        ["FormEditUser"]          = ["Edit User", "Редактировать пользователя"],
        ["SaveUser"]              = ["Save User", "Сохранить"],
        ["UserRoleLabel"]         = ["Role", "Роль"],
        ["Active"]                = ["Active", "Активен"],
        ["Inactive"]              = ["Inactive", "Неактивен"],
        ["LastLogin"]             = ["Last Login", "Последний вход"],
        ["LeaveBlankToKeep"]      = ["Leave blank to keep current password", "Оставьте пустым, чтобы не менять пароль"],
        ["CannotDeleteSelf"]      = ["Cannot delete your own account", "Нельзя удалить свою учётную запись"],
        ["AccessDenied"]          = ["Access denied", "Доступ запрещён"],
        ["ReadOnlyMode"]          = ["Read-only mode", "Режим только просмотра"],
    };

    public static string Get(string key)
    {
        if (!Strings.TryGetValue(key, out var pair))
            return key;
        return pair[(int)LocalizationService.Current];
    }

    public static string Format(string key, params object[] args)
        => string.Format(Get(key), args);

    public static string TranslateGender(string? value) => value switch
    {
        "Male"   => Get("Male"),
        "Female" => Get("Female"),
        _        => value ?? ""
    };

    public static string TranslateStatus(string? value) => value switch
    {
        "Scheduled"   => Get("StatusScheduled"),
        "Completed"   => Get("StatusCompleted"),
        "Cancelled"   => Get("StatusCancelled"),
        "Rescheduled" => Get("StatusRescheduled"),
        _             => value ?? ""
    };

    public static string GenderToDb(string? display) =>
        display == Get("Female") ? "Female" : "Male";

    public static string TranslateRole(string? role) => role switch
    {
        "Admin"        => Get("RoleAdmin"),
        "Doctor"       => Get("RoleDoctor"),
        "Receptionist" => Get("RoleReceptionist"),
        "Nurse"        => Get("RoleNurse"),
        "Manager"      => Get("RoleManager"),
        _              => role ?? ""
    };
}
