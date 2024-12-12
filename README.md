
# Online Bookstore

## Overview
This project implements a relational database design and C# console application for managing an online bookstore. The system allows handling books, authors, customers, and orders, providing essential CRUD operations for seamless database interaction. PostgreSQL is used as the relational database to store transactional data.
**(Update this section when Redis is implemented)**

---

## Database Schema

The database schema supports the following entities:
- **Authors**: Information about book authors.
- **Books**: Details of books, including pricing and stock levels.
- **Customers**: Data about customers placing orders.
- **Orders**: Records of customer purchases, including order details and totals.

### Schema Structure
The schema includes the following tables:
- `Authors`: Stores author information.
- `Books`: Stores book information with a reference to `Authors`.
- `Customers`: Stores customer information.
- `Orders`: Stores order information, linked to customers.
- `OrderDetails`: Stores individual items in an order, linked to books.

---

## Database Setup

### Prerequisites
1. Install PostgreSQL:
   ```bash
   brew install postgresql
   brew services start postgresql
   ```

2. Create the database:
   ```bash
   psql postgres
   CREATE DATABASE onlinebookstore;
   ```

3. Navigate to the project folder containing the `Database` folder.

### Import Schema and Seed Data
1. Execute the schema script to create tables:
   ```bash
   psql -d onlinebookstore -f Database/Schema.sql
   ```

2. Seed the database with sample data:
   ```bash
   psql -d onlinebookstore -f Database/SeedData.sql
   ```

---

## C# Console Application

### Overview
The C# console application interacts with the PostgreSQL database to manage books, authors, orders, and customers. The application uses `Entity Framework Core` for database interaction and implements the repository and service patterns.

### Features
1. **Add a New Book**:
    - Allows adding books to the `Books` table.
2. **View All Books**:
    - Displays all books, including stock levels and prices.
3. **Update Book Stock**:
    - Updates inventory levels for specific books.
4. **Place Orders**:
    - Records new orders and updates inventory levels.

### Files and Structure
- **Database Scripts**:
    - Located in the `Database` folder:
        - `Schema.sql`: Contains table creation statements.
        - `SeedData.sql`: Contains sample data.
- **C# Files**:
    - `AppDbContext`: Configures PostgreSQL database interaction.
    - `Models`: Defines entities (`Author`, `Book`, `Customer`, etc.).
    - `Repositories`: Contains CRUD logic for database tables.
    - `Services`: Contains business logic, e.g., `BookService`.
    - `Program.cs`: Main entry point, includes an interactive menu for user actions.

---

## Running the Application

### Prerequisites
1. Install .NET 9 SDK:
   ```bash
   brew install --cask dotnet
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```

### Running the Application
1. Navigate to the project folder:
   ```bash
   cd OnlineBookstore
   ```
2. Run the application:
   ```bash
   dotnet run
   ```

3. Follow the menu prompts to:
    - Add books.
    - View all books.
    - Update stock levels.

---

## Testing

### Database Queries
1. Test the SQL scripts in `psql` or a PostgreSQL GUI tool like pgAdmin.
2. Run the following test queries for accuracy and optimization:

#### **Test Queries**

Execute the schema script to create tables:
   ```bash
   psql -d onlinebookstore -f Database/TestQueries.sql
   ```

- Retrieve all books with authors:
   ```sql
   SELECT b.bookid, b.title, b.price, b.stock, a.name AS author_name
   FROM books b
            JOIN authors a ON b.authorid = a.authorid;
   ```

- Retrieve all orders with details:
   ```sql
   SELECT o.orderid, o.orderdate, o.totalamount, c.name AS customer_name, c.email
   FROM orders o
            JOIN customers c ON o.customerid = c.customerid;
   ```

- Retrieve total stock of books:
   ```sql
   SELECT SUM(stock) AS total_stock FROM books;
   ```

#### **Indexing and Optimization**
For faster queries, the following indexes are added in `Schema.sql`:
   ```sql
   CREATE INDEX idx_books_authorid ON books(authorid);
   CREATE INDEX idx_orders_customerid ON orders(customerid);
   CREATE INDEX idx_orderdetails_orderid ON orderdetails(orderid);
   CREATE INDEX idx_books_stock ON books(stock);
   CREATE INDEX idx_orders_orderdate ON orders(orderdate);
   ```

### Application Testing
1. Run the application with sample data.
2. Test the following scenarios:
    - Add a new book and verify it appears in the database.
    - Update stock for a book and confirm the changes.
    - View all books to ensure accurate data retrieval.

---

## Documentation

### Design Choices
1. **PostgreSQL**: Chosen for its relational capabilities and support for foreign keys.
2. **C# with Entity Framework Core**: Simplifies database interaction and supports LINQ queries.
3. **Repository Pattern**: Ensures separation of concerns between business logic and data access.

### Future Improvements
- Add Redis caching for frequently accessed data.
- Implement API endpoints for a web-based interface.

---

### Updating the Connection String

Before running the application, ensure that the connection string in `AppDbContext` matches your local PostgreSQL database configuration.

1. Open the `AppDbContext` file in the project.
2. Locate the `UseNpgsql` line in the `OnConfiguring` method:
   ```csharp
   optionsBuilder.UseNpgsql("Host=localhost;Database=onlinebookstore;Username=postgres;Password=yourpassword");
   ```
3. Update the following parts of the connection string as needed:
    - `Host`: The hostname or IP address of your PostgreSQL server (e.g., `localhost` or `127.0.0.1`).
    - `Database`: The name of your database (default is `onlinebookstore`).
    - `Username`: Your PostgreSQL username (default is `postgres`).
    - `Password`: Your PostgreSQL password.

4. Save the changes and re-run the application.

