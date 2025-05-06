using Microsoft.EntityFrameworkCore;
using SkillSnap.Data;
using SkillSnap.Models;

namespace SkillSnap.Services {
  public class PortfolioUserService {
    private readonly SkillSnapContext _context;
    public PortfolioUserService(SkillSnapContext context) {
      _context = context;
    }
    public async Task<List<PortfolioUser>> GetAllAsync() {
      return await _context.PortfolioUsers.Include(p => p.Projects).Include(p => p.Skills).ToListAsync();
    }

    public async Task<PortfolioUser?> GetByIdAsync(int id) {
      return await _context.PortfolioUsers.Include(p => p.Projects).Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(PortfolioUser user) {
      _context.PortfolioUsers.Add(user);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PortfolioUser user) {
      _context.PortfolioUsers.Update(user);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
      var user = await _context.PortfolioUsers.FindAsync(id);
      if(user != null) {
        _context.PortfolioUsers.Remove(user);
        await _context.SaveChangesAsync();
      }
    }
  }
}
