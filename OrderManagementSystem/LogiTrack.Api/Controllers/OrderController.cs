﻿using LogiTrack.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class OrderController: ControllerBase {
    private readonly LogiTrackContext _context;

    public OrderController(LogiTrackContext context) {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders() {
      return await _context.Orders.Include(o => o.Items).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id) {
      var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.OrderId == id);
      if(order == null) {
        return NotFound();
      }
      return order;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order) {
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