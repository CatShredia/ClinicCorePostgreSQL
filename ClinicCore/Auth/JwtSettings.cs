namespace ClinicCore.Auth;

public static class JwtSettings
{
    public const string Secret   = "ClinicCore_JWT_Secret_Key_2026_Min32Chars!";
    public const string Issuer   = "ClinicCore";
    public const string Audience = "ClinicCoreApp";
    public const int AccessTokenHours  = 8;
    public const int RefreshTokenDays  = 7;
}
