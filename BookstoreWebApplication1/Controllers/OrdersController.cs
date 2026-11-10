using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Data;

namespace BookstoreWebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

         public OrdersController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
    {
        return await _db.Orders.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id) {
        var order = await _db.Orders.FindAsync(id);
        if (order is null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order) {
        _db.Orders.Add(order); 
        await _db.SaveChangesAsync(); 
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            if (await _db.Orders.FindAsync(id) is Order order)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> PutOrder(int id, Order inputOrder)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order is null) return NotFound();
            order.OrderDate = inputOrder.OrderDate;
            order.OrderItems = inputOrder.OrderItems;
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
