using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using To_Do.Interfaces.Services.Authentication;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace To_Do.Services.Services.Authentication;

public class JwtTokenGenerator(IOptions<JwtSettings> jwtSettings) : IJwtTokenGenerator
{
    private readonly IOptions<JwtSettings> _jwtSettings = jwtSettings;

    public string GenerateJwtToken(Guid userId, string login, string email)
    {
        var signingCredentials =  new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Value.Secret)),
                    SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Nickname, login),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Value.Issuer,
            audience: _jwtSettings.Value.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredentials);
        
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}