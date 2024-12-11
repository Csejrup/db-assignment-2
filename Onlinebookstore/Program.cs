using Onlinebookstore.Repositories;
using Onlinebookstore.Services;

namespace Onlinebookstore
{
    internal abstract class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                await using var context = new AppDbContext();
                var bookRepo = new BookRepository(context);
                var bookService = new BookService(bookRepo);

                bool running = true;

                while (running)
                {
                    Console.Clear();
                    Console.WriteLine("=== Online Bookstore Menu ===");
                    Console.WriteLine("1. Add a New Book");
                    Console.WriteLine("2. View All Books");
                    Console.WriteLine("3. Update Book Stock");
                    Console.WriteLine("4. Exit");
                    Console.Write("Choose an option: ");

                    string? choice = Console.ReadLine();

                    try
                    {
                        switch (choice)
                        {
                            case "1":
                                await AddBookAsync(bookService);
                                break;
                            case "2":
                                await ViewBooksAsync(bookService);
                                break;
                            case "3":
                                await UpdateBookStockAsync(bookService);
                                break;
                            case "4":
                                running = false;
                                Console.WriteLine("Exiting...");
                                break;
                            default:
                                Console.WriteLine("Invalid option. Please try again.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    if (running)
                    {
                        Console.WriteLine("\nPress Enter to return to the menu...");
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical Error: {ex.Message}");
            }
        }

        private static async Task AddBookAsync(BookService bookService)
        {
            Console.Clear();
            Console.WriteLine("=== Add a New Book ===");

            try
            {
                Console.Write("Enter Book Title: ");
                string? title = Console.ReadLine();

                Console.Write("Enter Author ID: ");
                int authorId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Author ID is required."));

                Console.Write("Enter Price: ");
                decimal price = decimal.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Price is required."));

                Console.Write("Enter Stock: ");
                int stock = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Stock is required."));

                await bookService.AddBookAsync(title!, authorId, price, stock);
                Console.WriteLine("Book added successfully!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter the correct values.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding book: {ex.Message}");
            }
        }

        private static async Task ViewBooksAsync(BookService bookService)
        {
            Console.Clear();
            Console.WriteLine("=== Books Available ===");

            try
            {
                var books = await bookService.GetAllBooksAsync();
                foreach (var book in books)
                {
                    Console.WriteLine($"{book.BookID}: {book.Title} by Author {book.AuthorID} - ${book.Price} (Stock: {book.Stock})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving books: {ex.Message}");
            }
        }

        private static async Task UpdateBookStockAsync(BookService bookService)
        {
            Console.Clear();
            Console.WriteLine("=== Update Book Stock ===");

            try
            {
                Console.Write("Enter Book ID: ");
                int bookId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Book ID is required."));

                Console.Write("Enter Stock Change (positive or negative): ");
                int stockChange = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Stock change is required."));

                await bookService.UpdateStockAsync(bookId, stockChange);
                Console.WriteLine("Stock updated successfully!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating stock: {ex.Message}");
            }
        }
    }
}
