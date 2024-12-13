# Online Bookstore

## Overview

This project implements a relational database design and a C# console application for managing an online bookstore. The system uses PostgreSQL as the primary relational database for storing transactional data and integrates Redis for caching frequently accessed data to optimize performance.

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

3. Install and start Redis:
   ```bash
   brew install redis
   redis-server
   ```

### Import Schema and Seed Data

1. Execute the schema script to create tables:

   ```bash
   psql -d onlinebookstore -f onlinebookstore/Database/Schema.sql
   ```

2. Seed the database with sample data:
   ```bash
   psql -d onlinebookstore -f onlinebookstore/Database/SeedData.sql
   ```

---

## C# Console Application

### Features

1. **Add a New Book**:
   - Allows adding books to the `Books` table.
2. **View All Books**:
   - Displays all books, including stock levels and prices.
   - Utilizes Redis caching for frequently accessed data.
3. **Update Book Stock**:
   - Updates inventory levels for specific books.
   - Invalidates relevant Redis cache entries.
4. **Place Orders**:
   - Records new orders and updates inventory levels.
   - Verifies stock availability before creating orders.
5. **View Customer and Order Details**:
   - Displays order details, including associated books and quantities.
6. **View All Authors**:
   - Retrieves a list of all authors and their biographies.
7. **Test Cache Performance**:
   - Compares retrieval times with and without Redis caching.

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
  - `RedisCacheService`: Implements caching logic using Redis.
  - `Program.cs`: Main entry point, includes an interactive menu for user actions.

---

## Redis Integration

### Overview

Redis is used as a NoSQL database to cache frequently accessed data, reducing load on the PostgreSQL database and improving response times.

### Caching Strategy

1. **Books List**:
   - Cached with a 5-minute expiration.
   - Cache key: `List<Book>:all`.
2. **Book Details**:
   - Cached with a 1-hour expiration since book details rarely change.
   - Cache key: `Book:<BookID>`.

### Cache Invalidation

1. Cache for the books list (`all`) and individual book details (`Book:<BookID>`) is cleared whenever:
   - A book is added.
   - Stock levels are updated.

### Redis Configuration

Update the connection string in the `RedisCacheService` constructor:

```csharp
_redisConnection = ConnectionMultiplexer.Connect("localhost:6379");
```

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
   - Place orders.
   - View customer and order details.

---

## Testing

### Database Queries

1. Test the SQL scripts in `psql` or a PostgreSQL GUI tool like pgAdmin.
2. Run the following test queries for accuracy and optimization.

#### **Test Queries**

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

---

## Documentation

### Design Choices

1. **PostgreSQL**: Chosen for its relational capabilities and support for foreign keys.
2. **C# with Entity Framework Core**: Simplifies database interaction and supports LINQ queries.
3. **Redis**: Provides caching for frequently accessed data to improve application performance.

---

### Updating the Connection String

Before running the application, ensure that the connection string in `AppDbContext` matches your local PostgreSQL database configuration.

---
