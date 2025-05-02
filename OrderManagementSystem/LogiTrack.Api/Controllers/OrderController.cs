using LogiTrack.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LogiTrack.Api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class OrderController: ControllerBase {
    private readonly LogiTrackContext _context;
    private readonly IMemoryCache _cache;

    public OrderController(LogiTrackContext context, IMemoryCache cache) {
      _context = context;
      _cache = cache;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders() {
      if(!_cache.TryGetValue("OrdersCache", out List<Order> orders)) {
        orders = await _context.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .ToListAsync();

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        _cache.Set("OrdersCache", orders, cacheOptions);
      }

      return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id) {
      var order = await _context.Orders
          .Include(o => o.Items)
          .AsNoTracking() // Disable tracking for read-only queries
          .FirstOrDefaultAsync(o => o.OrderId == id);

      if(order == null) {
        return NotFound();
      }
      return order;
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<Order>> CreateOrder(Order order) {
      order.SessionId = HttpContext.Session.Id; // Store session ID
      _context.Orders.Add(order);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, Order order) {
      if(id != order.OrderId) {
        return BadRequest();
      }

      _context.Entry(order).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteOrder(int id) {
      var order = await _context.Orders.FindAsync(id);
      if(order == null) {
        return NotFound();
      }

      _context.Orders.Remove(order);
      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}