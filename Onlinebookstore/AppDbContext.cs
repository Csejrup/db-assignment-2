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

        // Author Entity
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.AuthorID).HasColumnName("authorid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Biography).HasColumnName("biography");

            // Author has many Books
            entity.HasMany(a => a.Books)
                  .WithOne(b => b.Author)
                  .HasForeignKey(b => b.AuthorID);
        });

        // Book Entity
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.BookID).HasColumnName("bookid");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.AuthorID).HasColumnName("authorid");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Stock).HasColumnName("stock");

            // Book has many OrderDetails
            entity.HasMany(b => b.OrderDetails)
                  .WithOne(od => od.Book)
                  .HasForeignKey(od => od.BookID);
        });

        // Customer Entity
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerID).HasColumnName("customerid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Address).HasColumnName("address");

            // Customer has many Orders
            entity.HasMany(c => c.Orders)
                  .WithOne(o => o.Customer)
                  .HasForeignKey(o => o.CustomerID);
        });

        // Order Entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderID).HasColumnName("orderid");
            entity.Property(e => e.CustomerID).HasColumnName("customerid");
            entity.Property(e => e.OrderDate).HasColumnName("orderdate");
            entity.Property(e => e.TotalAmount).HasColumnName("totalamount");

            // Order has many OrderDetails
            entity.HasMany(o => o.OrderDetails)
                  .WithOne(od => od.Order)
                  .HasForeignKey(od => od.OrderID);
        });

        // OrderDetail Entity
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.OrderDetailID).HasColumnName("orderdetailid");
            entity.Property(e => e.OrderID).HasColumnName("orderid");
            entity.Property(e => e.BookID).HasColumnName("bookid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Price).HasColumnName("price");

            // OrderDetail belongs to Order
            entity.HasOne(od => od.Order)
                  .WithMany(o => o.OrderDetails)
                  .HasForeignKey(od => od.OrderID);

            // OrderDetail belongs to Book
            entity.HasOne(od => od.Book)
                  .WithMany(b => b.OrderDetails)
                  .HasForeignKey(od => od.BookID);
        });
    }
}
