using Microsoft.Data.SqlClient;

namespace ClinicCore.Database;

public class DBHelper
{
    private static readonly string ConnStr =
        "Server=localhost,1433;Database=ClinicCoreDB;User Id=sa;" +
        "Password=YourPassword123!;TrustServerCertificate=True;";

    public static SqlConnection GetConnection()
    {
        var conn = new SqlConnection(ConnStr);
        conn.Open();
        return conn;
    }

    public static bool TestConnection()
    {
        try { using var conn = GetConnection(); return true; }
        catch { return false; }
    }
}
