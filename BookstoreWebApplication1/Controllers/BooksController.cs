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
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
         public BooksController(IBookService bookService) {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks(string? author,  string? isbn, string? name, 
        double? maxPrice, int? format, int? genre, bool available = false)
    {
        var result = await _bookService.GetAllBooks(author, isbn, name, maxPrice, format, genre, available);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id) {
        var book = await _bookService.GetBook(id);
        if (book is null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book) {
        var result = await _bookService.CreateBook(book);
        if (result == Enums.ServiceError.None) return Created();
        return BadRequest("Something went wrong");        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            if (!await _bookService.DeleteBook(id)) return NotFound();
            return NoContent();
        }

    [HttpPut("{id}")]
    public async Task<ActionResult<Book>> PutBook(int id, BookDTO inputBook)
    {
        var result = await _bookService.UpdateBook(id, inputBook);
        if (result == Enums.ServiceError.BookNotFound) return NotFound("Book does not exist");
        return NoContent();
    }

    }
}
