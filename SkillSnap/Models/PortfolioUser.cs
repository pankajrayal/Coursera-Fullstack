using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Models; 
public class PortfolioUser {
  [Key]
  public int Id { get; set; }

  [Required]
  public string Name { get; set; }
  public string Bio { get; set; }
  public string ProfileImageUrl { get; set; }
  public List<Project> Projects { get; set; } = new List<Project>();
  public List<Skill> Skills { get; set; } = new List<Skill>();
}
