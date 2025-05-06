using Microsoft.EntityFrameworkCore;
using SkillSnap.Data;
using SkillSnap.Models;

namespace SkillSnap.Services {
  public class SkillService {
    private readonly SkillSnapContext _context;
    public SkillService(SkillSnapContext context) {
      _context = context;
    }

    public async Task<List<Skill>> GetAllAsync() {
      return await _context.Skills.ToListAsync();
    }

    public async Task<Skill?> GetByIdAsync(int id) {
      return await _context.Skills.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Skill skill) {
      _context.Skills.Add(skill);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Skill skill) {
      _context.Skills.Update(skill);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
      var skill = await _context.Skills.FindAsync(id);
      if(skill != null) {
        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync();
      }
    }
  }
}
