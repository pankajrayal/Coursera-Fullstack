﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillSnap.Models;
public class Skill {
  [Key]
  public int Id { get; set; }

  [Required]
  public string Name { get; set; }
  public string Level { get; set; }

  [ForeignKey("PortfolioUser")]
  public int PortfolioUserId { get; set; }
  public PortfolioUser PortfolioUser { get; set; }
}
