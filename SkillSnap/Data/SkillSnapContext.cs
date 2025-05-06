﻿using Microsoft.EntityFrameworkCore;
using SkillSnap.Models;

namespace SkillSnap.Data {
  public class SkillSnapContext: DbContext {
    public SkillSnapContext(DbContextOptions<SkillSnapContext> options) : base(options) { }

    public DbSet<PortfolioUser> PortfolioUsers { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Skill> Skills { get; set; }
  }
}
