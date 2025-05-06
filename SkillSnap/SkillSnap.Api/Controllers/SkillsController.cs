using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SkillSnap.Api.Data;
using SkillSnap.Api.Models;

namespace SkillSnap.Api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class SkillsController: ControllerBase {
    private readonly SkillSnapContext _context;
    private readonly IMemoryCache _cache;

    public SkillsController(SkillSnapContext context, IMemoryCache cache) {
      _context = context;
      _cache = cache;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Skill>>> GetSkills() {
      const string cacheKey = "skills";

      if(!_cache.TryGetValue(cacheKey, out List<Skill> skills)) {
        skills = await _context.Skills.AsNoTracking().ToListAsync();

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetPriority(CacheItemPriority.Normal);

        _cache.Set(cacheKey, skills, cacheOptions);
      }

      return Ok(skills);
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