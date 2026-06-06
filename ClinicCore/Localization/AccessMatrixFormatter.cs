using System.Text;
using ClinicCore.Auth;

namespace ClinicCore.Localization;

public static class AccessMatrixFormatter
{
    private static readonly (string Resource, string LabelKey)[] Sections =
    [
        ("Dashboard",     "NavDashboard"),
        ("Patients",      "Patients"),
        ("Doctors",       "Doctors"),
        ("Appointments",  "Appointments"),
        ("Prescriptions", "Prescriptions"),
        ("Users",         "Users"),
    ];

    public static string ForRole(string? roleName)
    {
        var sb = new StringBuilder();
        foreach (var (resource, labelKey) in Sections)
        {
            var level = resource == "Dashboard"
                ? AccessLevel.Write
                : RolePermissions.GetAccess(roleName, resource);

            var levelKey = level switch
            {
                AccessLevel.Write => "AccessFull",
                AccessLevel.Read  => "AccessRead",
                _                 => "AccessNone"
            };

            sb.AppendLine($"{L.Get(labelKey)}: {L.Get(levelKey)}");
        }
        return sb.ToString().TrimEnd();
    }
}
