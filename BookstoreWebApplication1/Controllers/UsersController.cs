using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.DTOs;
using BookstoreWebApplication1.Services;
using BookstoreWebApplication1.Enums;

namespace BookstoreWebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

         public UsersController(IUserService userService) {
            _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        return Ok(await _userService.GetAllUsers());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id) {
        var user = await _userService.GetUser(id);
        if (user == null) return BadRequest("User not found");
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user) {
        var result = await _userService.CreateUser(user);
        if (result.Equals(ServiceError.None)) return Created();
        return BadRequest("Something went wrong");     
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> DeleteUser(int id)
        {
            if (await _userService.DeleteUser(id)) return NoContent();
            return BadRequest("User not found");
        }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> PutUser(int id, UserDTO inputUser)
    {
        var result = await _userService.UpdateUser(id, inputUser);
        if (result == ServiceError.UserNotFound) return BadRequest("User does not exist");

        return NoContent();
    }

    }
}
