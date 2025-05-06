using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SkillSnap.Api.Data;
using SkillSnap.Api.Models;
using System.Diagnostics;

namespace SkillSnap.Api.Controllers {
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class ProjectsController: ControllerBase {
    private readonly SkillSnapContext _context;
    private readonly IMemoryCache _cache;
    public ProjectsController(SkillSnapContext context, IMemoryCache cache) {
      _context = context;
      _cache = cache;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects() {
      const string cacheKey = "projects";
      Stopwatch stopwatch = Stopwatch.StartNew();

      if(!_cache.TryGetValue(cacheKey, out List<Project> projects)) {
        projects = await _context.Projects.AsNoTracking().ToListAsync();
        stopwatch.Stop();
        Console.WriteLine($"GetProjects Execution Time: {stopwatch.ElapsedMilliseconds} ms");

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetPriority(CacheItemPriority.Normal);

        _cache.Set(cacheKey, projects, cacheOptions);
      }

      return Ok(projects);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Project>> CreateProject(Project project) {
      _context.Projects.Add(project);
      await _context.SaveChangesAsync();
      return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
    }
  }
}