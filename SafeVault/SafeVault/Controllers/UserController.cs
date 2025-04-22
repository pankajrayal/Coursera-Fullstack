using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVault.Data;
using SafeVault.Models;
using SafeVault.Services;

namespace SafeVault.Controllers {
  public class UserController: Controller {
    private readonly SafeVaultDbContext _context;
    private readonly AuthenticationService _authService;

    public UserController(SafeVaultDbContext context) {
      _context = context;
      _authService = new AuthenticationService();
    }

    //[HttpPost]
    //public IActionResult Login(string username, string password) {
    //  var user = _context.Users.FirstOrDefault(u => u.Username == username);
    //  if(user == null || !_authService.VerifyPassword(password, user.PasswordHash)) {
    //    return Unauthorized("Invalid username or password.");
    //  }

    //  //// Example of a parameterized query
    //  //var user = _context.Users
    //  //    .FromSqlInterpolated($"SELECT * FROM Users WHERE Username = {username}")
    //  //    .FirstOrDefault();

    //  // Generate a session or token here (e.g., JWT)
    //  return Ok("Login successful.");
    //}

    [HttpPost]
    public IActionResult Login(string username, string password) {
      var sanitizedUsername = InputSanitizer.Sanitize(username);
      var user = _context.Users.FirstOrDefault(u => u.Username == sanitizedUsername);
      if(user == null || !_authService.VerifyPassword(password, user.PasswordHash)) {
        return Unauthorized("Invalid username or password.");
      }

      return Ok("Login successful.");
    }


    [Authorize(Roles = "Admin")]
    public IActionResult AdminDashboard() {
      return View();
    }

    [HttpPost]
    public IActionResult AssignRole(int userId, string role) {
      var user = _context.Users.Find(userId);
      if(user == null) {
        return NotFound("User not found.");
      }

      user.Role = role;
      _context.SaveChanges();

      return Ok("Role assigned successfully.");
    }

  }
}