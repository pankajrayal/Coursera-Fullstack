using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Api.Models {
  public class Skill {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Level { get; set; }

    // Foreign key relationship
    [ForeignKey("PortfolioUser")]
    public int PortfolioUserId { get; set; }
    public PortfolioUser PortfolioUser { get; set; }
  }
}