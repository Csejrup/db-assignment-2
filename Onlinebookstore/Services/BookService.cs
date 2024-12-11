using Onlinebookstore.Models;
using Onlinebookstore.Repositories;

namespace Onlinebookstore.Services;

public class BookService(BookRepository bookRepository)
{
    public async Task<List<Book>> GetAllBooksAsync()
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
    }

    public async Task UpdateStockAsync(int bookId, int quantity)
    {
        await bookRepository.UpdateBookStockAsync(bookId, quantity);
    }
}