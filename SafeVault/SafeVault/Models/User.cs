using System.ComponentModel.DataAnnotations;

namespace SafeVault.Models {
  public class User {
    [Key]
    public int UserID { get; set; }

    [Required]
    [StringLength(50)]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters allowed.")]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Role { get; set; } // e.g., "Admin", "User"
  }
}