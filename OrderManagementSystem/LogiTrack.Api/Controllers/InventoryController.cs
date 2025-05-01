using LogiTrack.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class InventoryController: ControllerBase {
    private readonly LogiTrackContext _context;

    public InventoryController(LogiTrackContext context) {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems() {
      return await _context.InventoryItems.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InventoryItem>> GetInventoryItem(int id) {
      var item = await _context.InventoryItems.FindAsync(id);
      if(item == null) {
        return NotFound();
      }
      return item;
    }

    [HttpPost]
    public async Task<ActionResult<InventoryItem>> CreateInventoryItem(InventoryItem item) {
      _context.InventoryItems.Add(item);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetInventoryItem), new { id = item.ItemId }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInventoryItem(int id, InventoryItem item) {
      if(id != item.ItemId) {
        return BadRequest();
      }

      _context.Entry(item).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventoryItem(int id) {
      var item = await _context.InventoryItems.FindAsync(id);
      if(item == null) {
        return NotFound();
      }

      _context.InventoryItems.Remove(item);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedData() {
      if(!_context.InventoryItems.Any()) {
        var inventoryItems = new List<InventoryItem>
        {
            new InventoryItem { Name = "Forklift", Quantity = 5, Location = "Warehouse B" },
            new InventoryItem { Name = "Pallet Jack", Quantity = 12, Location = "Warehouse A" },
            new InventoryItem { Name = "Shipping Labels", Quantity = 50, Location = "Office" }
        };

        _context.InventoryItems.AddRange(inventoryItems);
      }

      if(!_context.Orders.Any()) {
        var order = new Order {
          CustomerName = "Samir",
          DatePlaced = DateTime.UtcNow,
          Items = new List<InventoryItem>
            {
                new InventoryItem { Name = "Forklift", Quantity = 1, Location = "Warehouse B" },
                new InventoryItem { Name = "Shipping Labels", Quantity = 5, Location = "Office" }
            }
        };

        _context.Orders.Add(order);
      }

      await _context.SaveChangesAsync();

      return Ok("Test data seeded successfully.");
    }
  }
}