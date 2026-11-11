using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Data;

namespace BookstoreWebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

         public UsersController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        return await _db.Users.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id) {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user) {
        _db.Users.Add(user); 
        await _db.SaveChangesAsync(); 
        var basket = new Basket
        {
            UserId = user.Id,
            BasketItems = new()
        };
        _db.Baskets.Add(basket); 
        await _db.SaveChangesAsync(); 
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> DeleteUser(int id)
        {
            if (await _db.Users.FindAsync(id) is User user)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(int id, User inputUser)
        {
            var user = await _db.Users.FindAsync(id);
            if (user is null) return NotFound();
            user.Name = inputUser.Name;
            user.Email = inputUser.Email;
            user.Password = inputUser.Password;
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
