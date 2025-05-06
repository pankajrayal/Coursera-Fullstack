﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillSnap.Api.Data;
using SkillSnap.Api.Models;

namespace SkillSnap.Api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class ProjectsController: ControllerBase {
    private readonly SkillSnapContext _context;

    public ProjectsController(SkillSnapContext context) {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects() {
      return await _context.Projects.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(Project project) {
      _context.Projects.Add(project);
      await _context.SaveChangesAsync();
      return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
    }
  }
}