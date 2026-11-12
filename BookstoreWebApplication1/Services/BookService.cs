using BookstoreWebApplication1.Data;
using Microsoft.EntityFrameworkCore;

using BookstoreWebApplication1.DTOs;
using BookstoreWebApplication1.Models;
using BookstoreWebApplication1.Enums;

namespace BookstoreWebApplication1.Services;
public interface IBookService
{
    Task<Book?> GetBook(int bookId);
    Task<IEnumerable<Book>> GetAllBooks(string? author,  string? isbn, string? name, 
        double? maxPrice, int? format, int? genre, bool available);
    Task<ServiceError> UpdateBook(int bookId, BookDTO inputBook);
    Task<bool> DeleteBook(int bookId);
    Task<ServiceError> CreateBook(Book book);
}

public class BookService : IBookService
{
    private readonly ApplicationDbContext _db;
    public BookService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Book?> GetBook(int bookId)
    {
        var book = await _db.Books.FindAsync(bookId);
        return book;
    }

    public async Task<IEnumerable<Book>> GetAllBooks(string? author,  string? isbn, string? name, 
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
        return await books.ToListAsync();
    }

    public async Task<ServiceError> UpdateBook(int bookId, BookDTO inputBook)
    {
        var book = await _db.Books.FindAsync(bookId);
        if (book is null) return ServiceError.BookNotFound;
        // update book 
        if (!string.IsNullOrWhiteSpace(inputBook.Name)) book.Name = inputBook.Name;
        if (!string.IsNullOrWhiteSpace(inputBook.Author)) book.Author = inputBook.Author;
        if (!string.IsNullOrWhiteSpace(inputBook.ISBN)) book.ISBN = inputBook.ISBN;
        if (inputBook.Price != null) book.Price = (double) inputBook.Price;
        if (inputBook.Stock != null) book.Stock = (int) inputBook.Stock;
        if (inputBook.Format != null) book.Format = (BookFormat) inputBook.Format;
        if (inputBook.Genre != null) book.Genre = (BookGenre) inputBook.Genre;

        await _db.SaveChangesAsync();
        return ServiceError.None;

    }

    public async Task<bool> DeleteBook(int bookId) {
        if (await _db.Books.FindAsync(bookId) is Book book)
            {
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();
                return true;
            }
        return false;
    }

    public async Task<ServiceError> CreateBook(Book book)
    {
        _db.Books.Add(book); 
        await _db.SaveChangesAsync(); 
        return ServiceError.None;
        
    }
}