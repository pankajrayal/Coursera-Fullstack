using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Client.Models {
  public class Project {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }
    public string ImageUrl { get; set; }
  }
}
