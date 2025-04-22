using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthDemo.Services {
  public class TokenService {
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(IConfiguration config) {
      _secret = config["JwtSettings:Secret"];
      _issuer = config["JwtSettings:Issuer"];
      _audience = config["JwtSettings:Audience"];
    }

    public string GenerateToken(string username) {
      var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        issuer: _issuer,
        audience: _audience,
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}