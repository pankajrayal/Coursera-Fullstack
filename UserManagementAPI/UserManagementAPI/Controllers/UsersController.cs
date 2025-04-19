using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController: ControllerBase {
    private static List<User> users = new List<User>();

    // GET: api/users
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers() {
      return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public ActionResult<User> GetUserById(int id) {
      var user = users.FirstOrDefault(u => u.Id == id);
      return user != null ? Ok(user) : NotFound();
    }

    // POST: api/users
    [HttpPost]
    public ActionResult<User> CreateUser(User user) {
      user.Id = users.Count + 1;
      users.Add(user);
      return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, User updatedUser) {
      var user = users.FirstOrDefault(u => u.Id == id);
      if(user == null) return NotFound();

      user.Name = updatedUser.Name;
      user.Email = updatedUser.Email;
      user.Department = updatedUser.Department;

      return NoContent();
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id) {
      var user = users.FirstOrDefault(u => u.Id == id);
      if(user == null) return NotFound();

      users.Remove(user);
      return NoContent();
    }
  }
}
