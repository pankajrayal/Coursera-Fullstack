using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementAPI.Database;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController: ControllerBase {
    private static List<User> users = new List<User>();
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context) {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10) {
      try {
        if(page <= 0 || pageSize <= 0) {
          return BadRequest(new { Message = "Page and PageSize must be greater than 0." });
        }

        var totalUsers = await _context.Users.CountAsync();
        var paginatedUsers = await _context.Users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new {
          TotalCount = totalUsers,
          Page = page,
          PageSize = pageSize,
          Data = paginatedUsers
        });
      } catch(Exception ex) {
        Console.Error.WriteLine($"An error occurred in GetUsers: {ex.Message}");
        return StatusCode(500, new { Message = "An internal server error occurred." });
      }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id) {
      try {
        var user = await _context.Users.FindAsync(id);
        if(user == null) {
          Console.Error.WriteLine($"User with ID {id} not found.");
          return NotFound(new { Message = $"User with ID {id} not found." });
        }
        return Ok(user);
      } catch(Exception ex) {
        Console.Error.WriteLine($"An error occurred in GetUserById: {ex.Message}");
        return StatusCode(500, new { Message = "An internal server error occurred." });
      }
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] User user) {
      try {
        if(!ModelState.IsValid) {
          return BadRequest(ModelState);
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
      } catch(Exception ex) {
        Console.Error.WriteLine($"An error occurred in CreateUser: {ex.Message}");
        return StatusCode(500, new { Message = "An internal server error occurred." });
      }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser) {
      try {
        if(!ModelState.IsValid) {
          return BadRequest(ModelState);
        }

        var user = await _context.Users.FindAsync(id);
        if(user == null) return NotFound();

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.Department = updatedUser.Department;

        await _context.SaveChangesAsync();
        return NoContent();
      } catch(Exception ex) {
        Console.Error.WriteLine($"An error occurred in UpdateUser: {ex.Message}");
        return StatusCode(500, new { Message = "An internal server error occurred." });
      }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id) {
      try {
        var user = await _context.Users.FindAsync(id);
        if(user == null) return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
      } catch(Exception ex) {
        Console.Error.WriteLine($"An error occurred in DeleteUser: {ex.Message}");
        return StatusCode(500, new { Message = "An internal server error occurred." });
      }
    }

    [AllowAnonymous]
    [HttpPost("generate-token")]
    public IActionResult GenerateToken([FromBody] User user) {
      // Validate the user (this is a simple example, replace with actual validation logic)
      var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
      if(existingUser == null) {
        return Unauthorized(new { Message = "Invalid user credentials." });
      }

      // Access the configuration through a properly injected IConfiguration service
      var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
      if(string.IsNullOrEmpty(jwtKey)) {
        return StatusCode(500, new { Message = "JWT key is not configured." });
      }

      // Generate the token
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(jwtKey);
      var tokenDescriptor = new SecurityTokenDescriptor {
        Subject = new ClaimsIdentity(new[]
        {
          new Claim(ClaimTypes.Name, existingUser.Name),
          new Claim(ClaimTypes.Email, existingUser.Email)
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);

      return Ok(new { Token = tokenString });
    }
  }
}
