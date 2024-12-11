using Microsoft.EntityFrameworkCore;
using Onlinebookstore.Models;

namespace Onlinebookstore;

public class AppDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=onlinebookstore;Username=postgres;Password=password");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map tables to lowercase
        modelBuilder.Entity<Author>().ToTable("authors");
        modelBuilder.Entity<Book>().ToTable("books");
        modelBuilder.Entity<Customer>().ToTable("customers");
        modelBuilder.Entity<Order>().ToTable("orders");
        modelBuilder.Entity<OrderDetail>().ToTable("orderdetails");

        // Map columns to lowercase
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.AuthorID).HasColumnName("authorid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Biography).HasColumnName("biography");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.BookID).HasColumnName("bookid");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.AuthorID).HasColumnName("authorid");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Stock).HasColumnName("stock");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerID).HasColumnName("customerid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Address).HasColumnName("address");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderID).HasColumnName("orderid");
            entity.Property(e => e.CustomerID).HasColumnName("customerid");
            entity.Property(e => e.OrderDate).HasColumnName("orderdate");
            entity.Property(e => e.TotalAmount).HasColumnName("totalamount");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.OrderDetailID).HasColumnName("orderdetailid");
            entity.Property(e => e.OrderID).HasColumnName("orderid");
            entity.Property(e => e.BookID).HasColumnName("bookid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Price).HasColumnName("price");
        });
    }
}