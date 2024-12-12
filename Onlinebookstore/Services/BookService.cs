using Onlinebookstore.Models;
using Onlinebookstore.Repositories;

namespace Onlinebookstore.Services;

public class BookService(BookRepository bookRepository, RedisCacheService cacheService)
{
    public async Task<List<Book>> GetAllBooksAsync()
    {
        
        var cachedData = await cacheService.GetCacheAsync<List<Book>>("all");
        if (cachedData != null)
        {
            return cachedData;
        }
        
        var books = await bookRepository.GetAllBooksAsync();
        
        if (books.Any())
        {
            await cacheService.SetCacheAsync<List<Book>>("all", books, TimeSpan.FromMinutes(5));
        }

        return books;
    }
    
    public async Task<List<Book>> GetAllBooksAsyncWithoutCache()
    {
        
        return await bookRepository.GetAllBooksAsync();
    }

    public async Task AddBookAsync(string title, int authorId, decimal price, int stock)
    {
        
        
        var book = new Book
        {
            Title = title,
            AuthorID = authorId,
            Price = price,
            Stock = stock
        };
        await bookRepository.AddBookAsync(book);
        
        // Clear cache for list of books
        await cacheService.DeleteCacheAsync<List<Book>>("all");
    }
    public async Task<Book?> GetBookByIdAsync(int bookId)
    {
        // Check if data exists in the cache
        var cachedData = await cacheService.GetCacheAsync<Book>(bookId.ToString());
        if (cachedData != null)
        {
            return cachedData;
        }
        
        var book = await bookRepository.GetBookByIdAsync(bookId);
            
        // Store book in cachce if not null
        if (book != null)
        {
            // we add long cache timout here becouse book details not updated often
            await cacheService.SetCacheAsync<Book>(bookId.ToString(), book, new TimeSpan(1,0,0));
        }
        
        return book;
    }
    public async Task UpdateStockAsync(int bookId, int quantity)
    {
        
        // clear cache for book
        await cacheService.DeleteCacheAsync<Book>(bookId.ToString());
        
        // clear cache for list of books
        await cacheService.DeleteCacheAsync<List<Book>>("all");

        await bookRepository.UpdateBookStockAsync(bookId, quantity);
    }
}