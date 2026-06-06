using System;
using Npgsql;

namespace ClinicCore.Database;

public static class DBHelper
{
    private static readonly Lazy<string> ConnStr = new(BuildConnectionString);

    public static string ConnectionString => ConnStr.Value;
    public static string ServerLabel { get; private set; } = "localhost:5432";
    public static string DatabaseLabel { get; private set; } = "clinicdb";

    public static NpgsqlConnection GetConnection()
    {
        var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();
        return conn;
    }

    public static bool TestConnection()
    {
        try { using var conn = GetConnection(); return true; }
        catch { return false; }
    }

    private static string BuildConnectionString()
    {
        var direct = Environment.GetEnvironmentVariable("CLINICCORE_CONNECTION_STRING");
        if (!string.IsNullOrWhiteSpace(direct))
        {
            ApplyLabelsFromConnectionString(direct.Trim());
            if (direct.Contains("supabase.co", StringComparison.OrdinalIgnoreCase))
                ServerLabel = "Supabase";
            return direct.Trim();
        }

        var host = Environment.GetEnvironmentVariable("CLINICCORE_DB_HOST");
        if (!string.IsNullOrWhiteSpace(host))
        {
            var port = Environment.GetEnvironmentVariable("CLINICCORE_DB_PORT") ?? "5432";
            var database = Environment.GetEnvironmentVariable("CLINICCORE_DB_NAME") ?? "postgres";
            var user = Environment.GetEnvironmentVariable("CLINICCORE_DB_USER") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("CLINICCORE_DB_PASSWORD") ?? "";

            ServerLabel = host.Contains("supabase.co", StringComparison.OrdinalIgnoreCase)
                ? "Supabase"
                : $"{host}:{port}";
            DatabaseLabel = database;

            return $"Host={host};Port={port};Database={database};Username={user};Password={password};" +
                   "SSL Mode=Require;Trust Server Certificate=true";
        }

        ServerLabel = "localhost:5432";
        DatabaseLabel = "clinicdb";
        return "Host=localhost;Port=5432;Database=clinicdb;Username=postgres;Password=qwerty123";
    }

    private static void ApplyLabelsFromConnectionString(string connStr)
    {
        ServerLabel = GetValue(connStr, "Host") is { } h
            ? (h.Contains("supabase.co", StringComparison.OrdinalIgnoreCase) ? "Supabase" : $"{h}:{GetValue(connStr, "Port") ?? "5432"}")
            : "remote";
        DatabaseLabel = GetValue(connStr, "Database") ?? "postgres";
    }

    private static string? GetValue(string connStr, string key)
    {
        foreach (var part in connStr.Split(';', StringSplitOptions.RemoveEmptyEntries))
        {
            var idx = part.IndexOf('=');
            if (idx > 0 && part[..idx].Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                return part[(idx + 1)..].Trim();
        }
        return null;
    }
}
