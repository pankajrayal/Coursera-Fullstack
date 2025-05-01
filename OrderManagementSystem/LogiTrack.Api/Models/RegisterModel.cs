using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Api.Models {
  public class RegisterModel {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
  }
}
