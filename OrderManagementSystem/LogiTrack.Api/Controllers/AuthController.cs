using LogiTrack.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LogiTrack.Api.Controllers
{
  [Route("api/[controller]")]
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
      if(!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      var user = new ApplicationUser {
        UserName = model.Email,
        Email = model.Email,
        FirstName = model.FirstName,
        LastName = model.LastName
      };

      var result = await _userManager.CreateAsync(user, model.Password);

      if(!result.Succeeded) {
        return BadRequest(result.Errors);
      }

      return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model) {
      if(!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      var user = await _userManager.FindByEmailAsync(model.Email);
      if(user == null) {
        return Unauthorized("Invalid email or password.");
      }

      var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
      if(!result.Succeeded) {
        return Unauthorized("Invalid email or password.");
      }

      var token = GenerateJwtToken(user);
      return Ok(new { Token = token });
    }

    private string GenerateJwtToken(ApplicationUser user) {
      var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          _configuration["Jwt:Issuer"],
          _configuration["Jwt:Issuer"],
          claims,
          expires: DateTime.Now.AddHours(1),
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
