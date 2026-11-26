
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Services;
using BookstoreWebApplication1.Data;
using BookstoreWebApplication1.DTOs;

namespace BookstoreWebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // private readonly AppDbContext _context;
    private readonly IAuthService _authService;

    public AuthController( IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO request)
    {
        bool successful = await _authService.login(request);
        if (successful)
        {
            
            return Ok();
        }
        return BadRequest();

    }
}
