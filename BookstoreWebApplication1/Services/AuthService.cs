
using BookstoreWebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.DTOs;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Enums;
using BookstoreWebApplication1.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BookstoreWebApplication1.Services;

public interface IAuthService
{
    Task<bool> login(LoginDTO request);
}

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly TokenService _tokenService;

    public AuthService(ApplicationDbContext db, IPasswordHasher<User> passwordHasher, TokenService tokenService)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<bool> login(LoginDTO request) {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
            return false;

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.Password,
            request.Password
        );

        if (result == PasswordVerificationResult.Failed)
            return false;

        var token = _tokenService.GenerateToken(user);

        return true;
    }
}
