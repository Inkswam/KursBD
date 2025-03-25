using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace HotelManagementAPI.Services;

public class JwtService
{
    
    public string GenerateToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = "ThisIsNotAppropriateKeyThisIsNotAppropriateKeyThisIsNotAppropriateKey"u8.ToArray();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, email),
            new(JwtRegisteredClaimNames.Email, email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = "https://hotel.com",
            Audience = "https://hotel.com",
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public CookieOptions GetCookieOptions(string email)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTime.UtcNow.AddHours(3)
        };
    }
}