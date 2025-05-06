using Microsoft.EntityFrameworkCore;
using SkillSnap.Data;
using SkillSnap.Models;

namespace SkillSnap.Services {
  public class ProjectService {
    private readonly SkillSnapContext _context;
    public ProjectService(SkillSnapContext context) {
      _context = context;
    }

    public async Task<List<Project>> GetAllAsync() {
      return await _context.Projects.ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id) {
      return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Project project) {
      _context.Projects.Add(project);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Project project) {
      _context.Projects.Update(project);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
      var project = await _context.Projects.FindAsync(id);
      if(project != null) {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
      }
    }
  }
}
