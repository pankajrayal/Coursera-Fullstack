﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillSnap.Api.Data;
using SkillSnap.Api.Models;

namespace SkillSnap.Api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class SkillsController: ControllerBase {
    private readonly SkillSnapContext _context;

    public SkillsController(SkillSnapContext context) {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Skill>>> GetSkills() {
      return await _context.Skills.ToListAsync();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Skill>> CreateSkill(Skill skill) {
      _context.Skills.Add(skill);
      await _context.SaveChangesAsync();
      return CreatedAtAction(nameof(GetSkills), new { id = skill.Id }, skill);
    }
  }
}