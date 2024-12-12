using System.Diagnostics;
using Onlinebookstore.Models;
using Onlinebookstore.Repositories;
using Onlinebookstore.Services;

namespace Onlinebookstore;

internal abstract class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            await using var context = new AppDbContext();

            // Add RedisCacheService as a singleton to the DI container
            var redisCacheService = new RedisCacheService("redisConnectionString");

            var bookRepo = new BookRepository(context);
            var bookService = new BookService(bookRepo, redisCacheService);

            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== Online Bookstore Menu ===");
                Console.WriteLine("1. Add a New Book");
                Console.WriteLine("2. View All Books");
                Console.WriteLine("3. Update Book Stock");
                Console.WriteLine("4. Create a New Order");
                Console.WriteLine("5. View Customer and Order Details");
                Console.WriteLine("6. Test - Get all books time with and without cache");
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
                            await CreateOrderAsync(bookService, new OrderRepository(context));
                            break;
                        case "5":
                            await ViewCustomerOrdersAsync(new OrderRepository(context));
                            break;
                        case "6":
                            await TestCachePerformance(bookService);
                            break;
                        case "7":
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
    private static async Task CreateOrderAsync(BookService bookService, OrderRepository orderRepo)
    {
        Console.Clear();
        Console.WriteLine("=== Create a New Order ===");

        try
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Customer ID is required."));

            var orderDetails = new List<OrderDetail>();
            while (true)
            {
                Console.Write("Enter Book ID (or 0 to finish): ");
                int bookId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Book ID is required."));

                if (bookId == 0) break;

                Console.Write("Enter Quantity: ");
                int quantity = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Quantity is required."));

                var book = await bookService.GetBookByIdAsync(bookId);
                if (book == null || book.Stock < quantity)
                {
                    Console.WriteLine($"Not enough stock for book ID {bookId}. Try again.");
                    continue;
                }

                orderDetails.Add(new OrderDetail
                {
                    BookID = bookId,
                    Quantity = quantity,
                    Price = book.Price * quantity
                });

                await bookService.UpdateStockAsync(bookId, -quantity);
            }

            var totalAmount = orderDetails.Sum(od => od.Price);
            await orderRepo.CreateOrderAsync(customerId, totalAmount, orderDetails);

            Console.WriteLine("Order created successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating order: {ex.Message}");
        }
    }
    private static async Task ViewCustomerOrdersAsync(OrderRepository orderRepo)
    {
        Console.Clear();
        Console.WriteLine("=== View Customer and Order Details ===");

        try
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Customer ID is required."));

            var orders = await orderRepo.GetCustomerOrdersAsync(customerId);

            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.OrderID}, Total Amount: {order.TotalAmount}, Date: {order.OrderDate}");
            
                foreach (var detail in order.OrderDetails)
                {
                    Console.WriteLine($"  - Book ID: {detail.BookID}, Quantity: {detail.Quantity}, Price: ${detail.Price}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving customer orders: {ex.Message}");
        }
    }
    
    private static async Task TestCachePerformance(BookService bookService)
    {
        Console.Clear();

        try
        {
            
            // Just to fill out cache
            await bookService.GetAllBooksAsync();
            
            
            var stopwatchCache = Stopwatch.StartNew();
            await bookService.GetAllBooksAsync();
            stopwatchCache.Stop();

            var stopwatch = Stopwatch.StartNew();
            await bookService.GetAllBooksAsyncWithoutCache();
            stopwatch.Stop();
            
            Console.WriteLine($"Elapsed time without cache: {stopwatch.Elapsed} \n Elapsed time with cache: {stopwatchCache.Elapsed}");


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving customer orders: {ex.Message}");
        }
    }
}