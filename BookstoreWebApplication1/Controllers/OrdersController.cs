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
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders(int? userId)
    {

        if (userId != null)
            {
                var orders = from o in _db.Orders select o;
                orders = orders.Where(o => o.UserId == userId);
                return await orders.Include(o => o.OrderItems).ToListAsync();
            }
        return await _db.Orders.Include(o => o.OrderItems).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id) {
        var order = await _db.Orders.FindAsync(id);
        if (order is null) return NotFound();
        return Ok(order);
    }

    [HttpPost("{basketId}")]
    public async Task<ActionResult<Order>> CreateOrder(int basketId) {
        var basket = await _db.Baskets.Include(b => b.BasketItems).SingleOrDefaultAsync(b => b.Id == basketId);

        if (basket == null) return NotFound();
        if (basket.BasketItems == null) return BadRequest();

        var user = await _db.Users.FindAsync(basket.UserId); 

        if (user == null) return NotFound();

        var bookIds = basket.BasketItems.Select(item => item.BookId).Distinct().ToList();
        var books = await _db.Books.Where(book => bookIds.Contains(book.Id)).ToListAsync();

        var order = new Order
            {
                UserId = basket.UserId
            };

        foreach (var item in basket.BasketItems)
            {
                var book = await _db.Books.FindAsync(item.BookId); 
                if (book == null) return NotFound(); // book does not exist
                if (book.Stock < item.Quantity) return BadRequest(); // book out of stock

                var orderItem = new OrderItem
                {
                    BookId = book.Id,
                    Quantity = item.Quantity
                };
                book.Stock -= item.Quantity;
                // _db.OrderItems.Add(orderItem); 
                order.OrderItems.Add(orderItem);
            }

        basket.BasketItems.Clear();

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
