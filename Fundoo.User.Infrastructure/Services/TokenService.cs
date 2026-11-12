using System.Security.Claims;
using System.Text;
using Fundoo.User.Application.Interfaces;
using Fundoo.User.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Fundoo.User.Infrastructure.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string CreateAccessToken(AppUser user)
    {
        var secretKey = configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey is missing");
        var issuer = configuration["Jwt:Issuer"] ?? "fundoo-user-api";
        var audience = configuration["Jwt:Audience"] ?? "fundoo-clients";
        var expiryMinutes = int.TryParse(configuration["Jwt:ExpiryMinutes"], out var mins) ? mins : 30;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }),
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
}