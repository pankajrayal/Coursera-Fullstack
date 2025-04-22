using JwtAuthDemo.Models;
using JwtAuthDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthDemo.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class UserController: ControllerBase {
    private readonly TokenService _tokenService;
    private static readonly List<User> _users = new List<User>() {
      new User{Username = "prayal", Password = "password123"},
      new User{Username = "brayal", Password = "password123"},
      new User{Username = "arayal", Password = "password123"},
    };

    public UserController(TokenService tokenService) { 
      _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user) {
      var existing = _users.SingleOrDefault(u => u.Username == user.Username && u.Password == user.Password);
      if(existing == null) return Unauthorized("Invalid credentials");

      var token = _tokenService.GenerateToken(user.Username);
      return Ok(new { Token = token });
    }

    [HttpGet("secure-data")]
    [Authorize]
    public IActionResult GetSecureData() {
      return Ok(new { message = "You have accessed a secure endpoint!" });
    }
  }
}