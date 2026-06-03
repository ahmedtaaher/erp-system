using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth
{
  public class JwtProvider : IJwtProvider
  {
    private readonly JwtSettings _settings;
    public JwtProvider(IOptions<JwtSettings> settings)
    {
      _settings = settings.Value;
    }
    public string GenerateToken(User user)
    {
      var claims = new List<Claim>
      {
        new Claim("userId", user.Id.ToString()),
        new Claim("tenantId", user.TenantId.ToString()),
        new Claim("email", user.Email),
        new Claim(ClaimTypes.Role, user.Role)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        issuer: _settings.Issuer,
        audience: _settings.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(_settings.DurationInMinutes),
        signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}