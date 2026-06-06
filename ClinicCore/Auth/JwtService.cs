using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClinicCore.Models;
using Microsoft.IdentityModel.Tokens;

namespace ClinicCore.Auth;

public static class JwtService
{
    private static readonly SymmetricSecurityKey SigningKey =
        new(Encoding.UTF8.GetBytes(JwtSettings.Secret));

    private static readonly TokenValidationParameters ValidationParams = new()
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = JwtSettings.Issuer,
        ValidAudience            = JwtSettings.Audience,
        IssuerSigningKey         = SigningKey,
        ClockSkew                = TimeSpan.Zero
    };

    public static string GenerateAccessToken(AuthUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role),
            new("full_name", user.FullName)
        };

        var token = new JwtSecurityToken(
            issuer: JwtSettings.Issuer,
            audience: JwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(JwtSettings.AccessTokenHours),
            signingCredentials: new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string GenerateRefreshToken()
        => Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

    public static bool ValidateToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) return false;
        try
        {
            new JwtSecurityTokenHandler().ValidateToken(token, ValidationParams, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static AuthUser? GetUserFromToken(string? token)
    {
        if (!ValidateToken(token)) return null;
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            return new AuthUser
            {
                UserId   = int.Parse(jwt.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value),
                Username = jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value,
                Role     = jwt.Claims.First(c => c.Type == ClaimTypes.Role).Value,
                FullName = jwt.Claims.First(c => c.Type == "full_name").Value
            };
        }
        catch
        {
            return null;
        }
    }
}
