using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Api.Models {
  public class LogiTrackContext: IdentityDbContext<ApplicationUser> {
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Order> Orders { get; set; }

    // Constructor accepting DbContextOptions
    public LogiTrackContext(DbContextOptions<LogiTrackContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) {
      base.OnModelCreating(builder);
      // Additional configurations can be added here if needed
    }
  }
}