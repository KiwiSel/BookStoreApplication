using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Data;

namespace BookstoreWebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

         public BooksController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks(string? author,  string? isbn, string? name, 
        double? maxPrice, int? format, int? genre, bool available = false)
    {
        var books = _db.Books.AsQueryable();
        

        if (!string.IsNullOrWhiteSpace(isbn) ) books = books.Where(b => b.ISBN.Equals(isbn));
        if (!string.IsNullOrWhiteSpace(author)) books = books.Where(b => b.Author.ToLower().Replace(" ", "").Contains(author.ToLower().Replace(" ", "")));
        if (!string.IsNullOrWhiteSpace(name)) books = books.Where(b => b.Name.ToLower().Replace(" ", "").Contains(name.ToLower().Replace(" ", "")));
        if (format != null) books = books.Where(b => b.Format == (BookFormat) format);
        if (genre != null) books = books.Where(b => b.Genre == (BookGenre) genre);
        if (maxPrice != null) books = books.Where(b => b.Price <= maxPrice);
        if (available) books = books.Where(b => b.Stock > 0);

        return Ok(await books.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id) {
        var book = await _db.Books.FindAsync(id);
        if (book is null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book) {
        _db.Books.Add(book); 
        await _db.SaveChangesAsync(); 
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            if (await _db.Books.FindAsync(id) is Book book)
            {
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> PutBook(int id, Book inputBook)
        {
            var book = await _db.Books.FindAsync(id);
            if (book is null) return NotFound();
            book.Name = inputBook.Name;
            book.Author = inputBook.Author;
            book.ISBN = inputBook.ISBN;
            book.Price = inputBook.Price;
            book.Stock = inputBook.Stock;
            book.Format = inputBook.Format;
            book.Genre = inputBook.Genre;
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
