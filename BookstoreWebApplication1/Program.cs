using Microsoft.EntityFrameworkCore;
using BookstoreWebApplication1.Data;
using BookstoreWebApplication1.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// book endpoints 

app.MapGet("/books", async (ApplicationDbContext db) =>
    await db.Books.ToListAsync());

app.MapGet("/books/{id}", async (int id, ApplicationDbContext db) =>
    await db.Books.FindAsync(id)
        is Book book
            ? Results.Ok(book)
            : Results.NotFound());

app.MapPost("/books", async (Book book, ApplicationDbContext db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/books/{book.Id}", book);
});

app.MapDelete("/books/{id}", async (int id, ApplicationDbContext db) =>
{
    if (await db.Books.FindAsync(id) is Book book)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.MapPut("/books/{id}", async (int id, Book inputBook, ApplicationDbContext db) =>
{
    var book = await db.Books.FindAsync(id);

    if (book is null) return Results.NotFound();

    book.Name = inputBook.Name;
    book.Author = inputBook.Author;
    book.ISBN = inputBook.ISBN;
    book.Price = inputBook.Price;
    book.Stock = inputBook.Stock;
    book.Format = inputBook.Format;
    book.Genre = inputBook.Genre;

    await db.SaveChangesAsync();

    return Results.NoContent();
});



// user endpoints 

app.MapGet("/users", async (ApplicationDbContext db) =>
    await db.Users.ToListAsync());

app.MapGet("/users/{id}", async (int id, ApplicationDbContext db) =>
    await db.Users.FindAsync(id)
        is User user
            ? Results.Ok(user)
            : Results.NotFound());

app.MapPost("/users", async (User user, ApplicationDbContext db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
});

app.MapDelete("/users/{id}", async (int id, ApplicationDbContext db) =>
{
    if (await db.Users.FindAsync(id) is User user)
    {
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.MapPut("/users/{id}", async (int id, User inputUser, ApplicationDbContext db) =>
{
    var user = await db.Users.FindAsync(id);

    if (user is null) return Results.NotFound();

    user.Name = inputUser.Name;
    user.Email = inputUser.Email;
    user.Password = inputUser.Password;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.UseHttpsRedirection();
app.Run();