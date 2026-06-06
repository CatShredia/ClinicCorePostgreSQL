namespace ClinicCore.Auth;

public enum AccessLevel
{
    None,
    Read,
    Write
}

public static class RolePermissions
{
    public static AccessLevel GetAccess(string? role, string resource) => (role ?? "", resource) switch
    {
        ("Admin", _) => AccessLevel.Write,

        ("Manager", "Users") => AccessLevel.None,
        ("Manager", _)        => AccessLevel.Write,

        ("Doctor", "Patients" or "Doctors") => AccessLevel.Read,
        ("Doctor", "Appointments" or "Prescriptions") => AccessLevel.Write,
        ("Doctor", _) => AccessLevel.None,

        ("Receptionist", "Doctors") => AccessLevel.Read,
        ("Receptionist", "Prescriptions" or "Users") => AccessLevel.None,
        ("Receptionist", _) => AccessLevel.Write,

        ("Nurse", "Users") => AccessLevel.None,
        ("Nurse", _) => AccessLevel.Read,

        _ => AccessLevel.None
    };

    public static bool CanViewPage(string? role, string page) => page switch
    {
        "Dashboard" => true,
        "Users"       => GetAccess(role, "Users") != AccessLevel.None,
        "Patients"    => GetAccess(role, "Patients") != AccessLevel.None,
        "Doctors"     => GetAccess(role, "Doctors") != AccessLevel.None,
        "Appointments"=> GetAccess(role, "Appointments") != AccessLevel.None,
        "Prescriptions"=> GetAccess(role, "Prescriptions") != AccessLevel.None,
        _ => false
    };

    public static bool CanWrite(string? role, string resource)
        => GetAccess(role, resource) == AccessLevel.Write;
}
