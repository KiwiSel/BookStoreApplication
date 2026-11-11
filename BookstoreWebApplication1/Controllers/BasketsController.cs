using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Data;

namespace BookstoreWebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

         public BasketsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Basket>>> GetAllBaskets(int? userId)
    {

        if (userId != null) return Ok(_db.Baskets.Where(ba=>ba.UserId == userId).
            Include(ba => ba.BasketItems).ToListAsync());

        return Ok(await _db.Baskets.Include(ba => ba.BasketItems).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Basket>> GetBasket(int id) {
        var basket = await _db.Baskets.FindAsync(id);
        if (basket is null) return NotFound();
        return Ok(basket);
    }

    [HttpPut("{id}")]
        public async Task<ActionResult<Basket>> PutBasketItem(int id, BasketItem inputBasketItem)
        {
            var basket = await _db.Baskets.FindAsync(id);
            if (basket is null) return NotFound();

            var basketItem = new BasketItem
            {
                Quantity = inputBasketItem.Quantity,
                BasketId = basket.Id,
                BookId = inputBasketItem.BookId
                
            };

            basket.BasketItems.Add(basketItem);
            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}
