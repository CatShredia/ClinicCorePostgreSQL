using ClinicCore.Models;

namespace ClinicCore.Auth;

public static class AuthSession
{
    public static AuthUser? CurrentUser { get; private set; }
    public static string? AccessToken { get; private set; }
    public static string? RefreshToken { get; private set; }

    public static bool IsAuthenticated =>
        CurrentUser != null && JwtService.ValidateToken(AccessToken);

    public static void SetSession(AuthResult result)
    {
        CurrentUser   = result.User;
        AccessToken   = result.AccessToken;
        RefreshToken  = result.RefreshToken;
    }

    public static void Logout()
    {
        AuthService.RevokeRefreshToken(RefreshToken);
        CurrentUser  = null;
        AccessToken  = null;
        RefreshToken = null;
    }
}
