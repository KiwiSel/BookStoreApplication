using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Data;
using BookstoreWebApplication1.DTOs;
using BookstoreWebApplication1.Services;

namespace BookstoreWebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;

         public BasketsController(ApplicationDbContext db, IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Basket>>> GetAllBaskets(int? userId) {
        var baskets = await _basketService.GetAllBaskets(userId);
        return Ok(baskets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Basket>> GetBasket(int id) {
        var basket = await _basketService.GetBasket(id);
        if (basket is null) return NotFound();
        return Ok(basket);
    }

    [HttpPut("{id}")]
        public async Task<ActionResult<Basket>> PutBasketItem(int id, BasketItem inputBasketItem)
        {
            var result = await _basketService.AddBasketItem(id, inputBasketItem);
            if (result == Enums.ServiceError.BasketNotFound) return NotFound("Basket not configured");
            return NoContent();
        }

    }
}
