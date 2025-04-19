using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace UserManagementAPI.Middlewares {
  public class AuthenticationMiddleware {
    private readonly RequestDelegate _next;
    private readonly string _secretKey;

    public AuthenticationMiddleware(RequestDelegate next, string secretKey) {
      _next = next;
      _secretKey = secretKey;
    }

    public async Task InvokeAsync(HttpContext context) {
      var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

      if(string.IsNullOrEmpty(token) || !ValidateToken(token)) {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized: Invalid or missing token.");
        return;
      }

      // Proceed to the next middleware
      await _next(context);
    }

    private bool ValidateToken(string token) {
      try {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        tokenHandler.ValidateToken(token, new TokenValidationParameters {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = true
        }, out _);

        return true;
      } catch {
        return false;
      }
    }
  }
}
