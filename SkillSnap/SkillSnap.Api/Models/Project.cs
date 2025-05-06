﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Api.Models {
  public class Project {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }
    public string ImageUrl { get; set; }

    // Foreign key relationship
    [ForeignKey("PortfolioUser")]
    public int PortfolioUserId { get; set; }
    public PortfolioUser PortfolioUser { get; set; }
  }
}