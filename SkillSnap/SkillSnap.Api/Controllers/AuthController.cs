using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SkillSnap.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkillSnap.Api.Controllers {
  [Route("api/auth")]
  [ApiController]
  public class AuthController: ControllerBase {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) {
      _userManager = userManager;
      _signInManager = signInManager;
      _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model) {
      var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
      var result = await _userManager.CreateAsync(user, model.Password);

      if(!result.Succeeded) {
        return BadRequest(result.Errors);
      }

      return Ok("User registered successfully.");
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model) {
      var user = await _userManager.FindByEmailAsync(model.Email);
      if(user == null || !(await _userManager.CheckPasswordAsync(user, model.Password))) {
        return Unauthorized("Invalid credentials.");
      }

      var token = GenerateJwtToken(user);
      return Ok(new { Token = token });
    }


    private string GenerateJwtToken(ApplicationUser user) {
      var claims = new[]
      {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var token = new JwtSecurityToken(
          issuer: _configuration["JwtSettings:Issuer"],
          audience: _configuration["JwtSettings:Audience"],
          claims: claims,
          expires: DateTime.UtcNow.AddHours(2),
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}