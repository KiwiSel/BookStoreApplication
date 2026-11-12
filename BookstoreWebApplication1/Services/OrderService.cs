using BookstoreWebApplication1.Data;
using Microsoft.EntityFrameworkCore;

using BookstoreWebApplication1.DTOs;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Enums;

namespace BookstoreWebApplication1.Services;
public interface IOrderService
{
    Task<Order?> GetOrder(int orderId);
    Task<IEnumerable<Order>> GetAllOrders(int? userId);
    Task<ServiceError> UpdateOrder(int orderId, OrderDTO inputOrder);
    Task<bool> DeleteOrder(int orderId);
    Task<ServiceError> CreateOrder(int basketId);
}

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _db;
    public OrderService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Order?> GetOrder(int orderId)
    {
        var order = await _db.Orders.Include(o => o.OrderItems).SingleOrDefaultAsync(o => o.Id == orderId);
        return order;
    }

    public async Task<IEnumerable<Order>> GetAllOrders(int? userId)
    {
        if (userId != null)
            {
                var orders = from o in _db.Orders select o;
                orders = orders.Where(o => o.UserId == userId);
                return await orders.Include(o => o.OrderItems).ToListAsync();
            }
        return await _db.Orders.Include(o => o.OrderItems).ToListAsync();
    }

    public async Task<ServiceError> UpdateOrder(int orderId, OrderDTO inputOrder)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order is null) return ServiceError.OrderNotFound;
        
        if (inputOrder.OrderDate != null) order.OrderDate = (DateTime) inputOrder.OrderDate;
        await _db.SaveChangesAsync();
        return ServiceError.None;

    }

    public async Task<bool> DeleteOrder(int orderId) {
        if (await _db.Orders.FindAsync(orderId) is Order order)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
                return true;
            }
        return false;
    }

    public async Task<ServiceError> CreateOrder(int basketId)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync();

        var basket = await _db.Baskets.Include(b => b.BasketItems).SingleOrDefaultAsync(b => b.Id == basketId);
        if (basket == null) return ServiceError.BasketNotFound;
        if (basket.BasketItems == null || !basket.BasketItems.Any()) return ServiceError.EmptyBasket;

        var user = await _db.Users.FindAsync(basket.UserId); 

        if (user == null) return ServiceError.UserNotFound;

        var order = new Order
            {
                UserId = basket.UserId,
                OrderDate = DateTime.Now
            };

        foreach (var item in basket.BasketItems)
            {
                var book = await _db.Books.AsNoTracking().SingleOrDefaultAsync(b => b.Id == item.BookId); 
                if (book == null) return ServiceError.BookNotFound;
   
                var updated = await _db.Books.Where(b => b.Id == item.BookId && b.Stock >= item.Quantity).
                    ExecuteUpdateAsync(setters => setters.SetProperty(b => b.Stock, b => b.Stock - item.Quantity));
                if (updated == 0) return ServiceError.InsufficientStock;
                
                var orderItem = new OrderItem
                {
                    BookId = book.Id,
                    Quantity = item.Quantity,
                    Price = book.Price * item.Quantity
                };
                // _db.OrderItems.Add(orderItem); 
                order.OrderItems.Add(orderItem);
                order.Price += orderItem.Price;
            }

        basket.BasketItems.Clear();

        _db.Orders.Add(order); 

        await _db.SaveChangesAsync(); 
        await transaction.CommitAsync();

        return ServiceError.None;
        
    }
}