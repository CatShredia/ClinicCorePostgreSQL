using System;
using ClinicCore.Database;
using ClinicCore.Models;
using Npgsql;

namespace ClinicCore.Auth;

public static class AuthService
{
    public static AuthResult? Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return null;

        try
        {
            using var conn = DBHelper.GetConnection();
            using var cmd = new NpgsqlCommand(@"
                SELECT u.""UserID"", u.""Username"", u.""FullName"", r.""RoleName""
                FROM ""Users"" u
                JOIN ""Roles"" r ON u.""RoleID"" = r.""RoleID""
                WHERE u.""Username"" = @u
                  AND u.""PasswordHash"" = crypt(@p, u.""PasswordHash"")
                  AND u.""IsActive"" = TRUE", conn);

            cmd.Parameters.AddWithValue("@u", username.Trim());
            cmd.Parameters.AddWithValue("@p", password);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            var user = new AuthUser
            {
                UserId   = reader.GetInt32(0),
                Username = reader.GetString(1),
                FullName = reader.GetString(2),
                Role     = reader.GetString(3)
            };
            reader.Close();

            var accessToken  = JwtService.GenerateAccessToken(user);
            var refreshToken = JwtService.GenerateRefreshToken();
            SaveRefreshToken(conn, user.UserId, refreshToken);
            UpdateLastLogin(conn, user.UserId);

            return new AuthResult(user, accessToken, refreshToken);
        }
        catch
        {
            return null;
        }
    }

    public static void RevokeRefreshToken(string? refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken)) return;
        try
        {
            using var conn = DBHelper.GetConnection();
            using var cmd = new NpgsqlCommand(@"
                UPDATE ""RefreshTokens""
                SET ""IsRevoked"" = TRUE
                WHERE ""Token"" = @t", conn);
            cmd.Parameters.AddWithValue("@t", refreshToken);
            cmd.ExecuteNonQuery();
        }
        catch { }
    }

    private static void SaveRefreshToken(NpgsqlConnection conn, int userId, string token)
    {
        using var revoke = new NpgsqlCommand(@"
            UPDATE ""RefreshTokens"" SET ""IsRevoked"" = TRUE
            WHERE ""UserID"" = @uid AND ""IsRevoked"" = FALSE", conn);
        revoke.Parameters.AddWithValue("@uid", userId);
        revoke.ExecuteNonQuery();

        using var cmd = new NpgsqlCommand(@"
            INSERT INTO ""RefreshTokens"" (""UserID"", ""Token"", ""ExpiresAt"")
            VALUES (@uid, @t, @exp)", conn);
        cmd.Parameters.AddWithValue("@uid", userId);
        cmd.Parameters.AddWithValue("@t", token);
        cmd.Parameters.AddWithValue("@exp", DateTime.UtcNow.AddDays(JwtSettings.RefreshTokenDays));
        cmd.ExecuteNonQuery();
    }

    private static void UpdateLastLogin(NpgsqlConnection conn, int userId)
    {
        using var cmd = new NpgsqlCommand(@"
            UPDATE ""Users"" SET ""LastLoginAt"" = CURRENT_TIMESTAMP
            WHERE ""UserID"" = @id", conn);
        cmd.Parameters.AddWithValue("@id", userId);
        cmd.ExecuteNonQuery();
    }
}

public record AuthResult(AuthUser User, string AccessToken, string RefreshToken);
