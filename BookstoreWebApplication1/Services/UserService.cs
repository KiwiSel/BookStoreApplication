using BookstoreWebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.DTOs;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Enums;

namespace BookstoreWebApplication1.Services;
public interface IUserService
{
    Task<User?> GetUser(int userId);
    Task<IEnumerable<User>> GetAllUsers();
    Task<ServiceError> UpdateUser(int userId, UserDTO inputUser);
    Task<bool> DeleteUser(int userId);
    Task<ServiceError> CreateUser(User user);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _db;
    public UserService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetUser(int userId)
    {
        var user = await _db.Users.FindAsync(userId);
        return user;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task<ServiceError> UpdateUser(int userId, UserDTO inputUser)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return ServiceError.UserNotFound;

        // update user 
        if (!string.IsNullOrWhiteSpace(inputUser.Name)) user.Name = inputUser.Name;
        if (!string.IsNullOrWhiteSpace(inputUser.Email)) user.Email = inputUser.Email;
        if (!string.IsNullOrWhiteSpace(inputUser.Password)) user.Password = inputUser.Password;

        await _db.SaveChangesAsync();
        return ServiceError.None;

    }

    public async Task<bool> DeleteUser(int userId) {
        if (await _db.Users.FindAsync(userId) is User user)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return true;
            }
        return false;
    }

    public async Task<ServiceError> CreateUser(User user)
    {
        _db.Users.Add(user); 
        await _db.SaveChangesAsync(); 
        var basket = new Basket
        {
            UserId = user.Id,
            BasketItems = new()
        };
        _db.Baskets.Add(basket); 
        await _db.SaveChangesAsync(); 
        return ServiceError.None;
        
    }
}