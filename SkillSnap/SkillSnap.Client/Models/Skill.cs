using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Client.Models {
  public class Skill {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Level { get; set; }
  }
}