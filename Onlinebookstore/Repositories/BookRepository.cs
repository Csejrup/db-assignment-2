using Microsoft.EntityFrameworkCore;
using Onlinebookstore.Models;

namespace Onlinebookstore.Repositories;

public class BookRepository(AppDbContext context)
{
    public async Task<List<Book?>> GetAllBooksAsync()
    {
        return await context.Books.Include(b => b.Author).ToListAsync();
    }

    public async Task AddBookAsync(Book? book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
    }
    public async Task<Book?> GetBookByIdAsync(int bookId)
    {
        return await context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.BookID == bookId);
    }
    public async Task UpdateBookStockAsync(int bookId, int quantity)
    {
        var book = await context.Books.FindAsync(bookId);
        if (book != null)
        {
            book.Stock -= quantity;
            await context.SaveChangesAsync();
        }
    }
}