
using BookstoreWebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.DTOs;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Enums;

namespace BookstoreWebApplication1.Services;

public interface IBasketService
{
    Task<IEnumerable<Basket>> GetAllBaskets(int? userId);
    Task<Basket?> GetBasket(int basketId);
    Task<ServiceError> AddBasketItem(int basketId, BasketItem inputBasketItem);
}

public class BasketService : IBasketService
{
    private readonly ApplicationDbContext _db;

    public BasketService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Basket>> GetAllBaskets(int? userId)
    {
        if (userId != null)
        {
            return await _db.Baskets
                .Where(ba => ba.UserId == userId)
                .Include(ba => ba.BasketItems)
                .ToListAsync();
        }

        return await _db.Baskets
            .Include(ba => ba.BasketItems)
            .ToListAsync();
    }

    public async Task<Basket?> GetBasket(int basketId)
    {
        var basket = await _db.Baskets
            .Include(ba => ba.BasketItems)
            .FirstOrDefaultAsync(ba => ba.Id == basketId);

        return basket;
    }

    public async Task<ServiceError> AddBasketItem(int basketId, BasketItem inputBasketItem)
    {
        var basket = await _db.Baskets.FindAsync(basketId);
        if (basket is null) return ServiceError.BasketNotFound;

        var basketItem = new BasketItem
        {
            Quantity = inputBasketItem.Quantity,
            BasketId = basket.Id,
            BookId = inputBasketItem.BookId
        };

        basket.BasketItems.Add(basketItem);

        await _db.SaveChangesAsync();
        return ServiceError.None;
    }
}
