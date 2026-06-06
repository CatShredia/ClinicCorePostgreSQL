using Npgsql;

namespace ClinicCore.Database;

public class DBHelper
{
    private static readonly string ConnStr =
            "Host=localhost;Port=5432;Database=clinicdb;Username=postgres;Password=qwerty123";

    public static NpgsqlConnection GetConnection()
    {
        var conn = new NpgsqlConnection(ConnStr);
        conn.Open();
        return conn;
    }

    public static bool TestConnection()
    {
        try { using var conn = GetConnection(); return true; }
        catch { return false; }
    }
}
