using System.Collections.Generic;
using ClinicCore.Database;
using ClinicCore.Localization;
using ClinicCore.Models;
using Npgsql;

namespace ClinicCore.Services;

public static class UserService
{
    public static List<UserAccount> GetAll()
    {
        var list = new List<UserAccount>();
        using var conn = DBHelper.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            SELECT u.""UserID"", u.""Username"", u.""FullName"", r.""RoleName"", u.""RoleID"",
                   u.""IsActive"", u.""LastLoginAt""
            FROM ""Users"" u
            JOIN ""Roles"" r ON u.""RoleID"" = r.""RoleID""
            ORDER BY u.""UserID""", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var active = reader.GetBoolean(5);
            list.Add(new UserAccount
            {
                UserID      = reader.GetInt32(0),
                Username    = reader.GetString(1),
                FullName    = reader.GetString(2),
                RoleName    = reader.GetString(3),
                RoleID      = reader.GetInt32(4),
                IsActive    = active,
                ActiveStatus = active ? L.Get("Active") : L.Get("Inactive"),
                LastLoginAt = reader.IsDBNull(6) ? "" : reader.GetDateTime(6).ToString("yyyy-MM-dd HH:mm")
            });
        }
        return list;
    }

    public static List<RoleItem> GetRoles()
    {
        var list = new List<RoleItem>();
        using var conn = DBHelper.GetConnection();
        using var cmd = new NpgsqlCommand(@"SELECT ""RoleID"", ""RoleName"" FROM ""Roles"" ORDER BY ""RoleID""", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new RoleItem { RoleID = reader.GetInt32(0), RoleName = reader.GetString(1) });
        return list;
    }

    public static void Create(string username, string password, string fullName, int roleId)
    {
        using var conn = DBHelper.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            INSERT INTO ""Users"" (""Username"", ""PasswordHash"", ""FullName"", ""RoleID"", ""IsActive"")
            VALUES (@u, crypt(@p, gen_salt('bf')), @n, @r, TRUE)", conn);
        cmd.Parameters.AddWithValue("@u", username.Trim());
        cmd.Parameters.AddWithValue("@p", password);
        cmd.Parameters.AddWithValue("@n", fullName.Trim());
        cmd.Parameters.AddWithValue("@r", roleId);
        cmd.ExecuteNonQuery();
    }

    public static void Update(int userId, string fullName, int roleId, bool isActive, string? newPassword)
    {
        using var conn = DBHelper.GetConnection();
        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            using var cmd = new NpgsqlCommand(@"
                UPDATE ""Users"" SET ""FullName""=@n, ""RoleID""=@r, ""IsActive""=@a,
                    ""PasswordHash""=crypt(@p, gen_salt('bf'))
                WHERE ""UserID""=@id", conn);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.Parameters.AddWithValue("@n", fullName.Trim());
            cmd.Parameters.AddWithValue("@r", roleId);
            cmd.Parameters.AddWithValue("@a", isActive);
            cmd.Parameters.AddWithValue("@p", newPassword);
            cmd.ExecuteNonQuery();
        }
        else
        {
            using var cmd = new NpgsqlCommand(@"
                UPDATE ""Users"" SET ""FullName""=@n, ""RoleID""=@r, ""IsActive""=@a
                WHERE ""UserID""=@id", conn);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.Parameters.AddWithValue("@n", fullName.Trim());
            cmd.Parameters.AddWithValue("@r", roleId);
            cmd.Parameters.AddWithValue("@a", isActive);
            cmd.ExecuteNonQuery();
        }
    }

    public static void Delete(int userId)
    {
        using var conn = DBHelper.GetConnection();
        using var cmd = new NpgsqlCommand(@"DELETE FROM ""Users"" WHERE ""UserID""=@id", conn);
        cmd.Parameters.AddWithValue("@id", userId);
        cmd.ExecuteNonQuery();
    }
}
