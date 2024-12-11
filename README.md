
# Relational Database for Online Bookstore

## Overview
This part of the project focuses on the design and implementation of the relational database for the Online Bookstore application. PostgreSQL is used as the relational database to store core transactional data such as orders, customers, books, and authors.

---

## Database Schema

The relational database schema is designed to handle the following entities:
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
   CREATE DATABASE OnlineBookstore;
   ```

### Import Schema and Seed Data
1. Navigate to the `Database` folder:
   ```bash
   cd OnlineBookstore/Database
   ```

2. Execute the schema script:
   ```bash
   psql -d onlinebookstore -f Schema.sql
   ```

3. Seed the database with sample data:
   ```bash
   psql -d OnlineBookstore -f SeedData.sql
   ```

---

## CRUD Operations

The following operations are implemented in the application:
1. **Create New Orders**:
    - Adds a new order to the `Orders` table.
    - Updates the inventory in the `Books` table.

2. **Retrieve Customer and Order Details**:
    - Fetches customer information and their associated orders.

3. **Update Inventory**:
    - Reduces stock levels when an order is placed.

---

## Contribution Notes

### Files and Structure
- **SQL Scripts**:
    - Located in the `Database` folder.
    - `Schema.sql`: Contains table creation statements.
    - `SeedData.sql`: Contains sample data.

- **C# CRUD Operations**:
    - Implemented in `Repositories` for database interaction.
    - Business logic for inventory and orders is in `Services`.

### Commit History
Make incremental commits for:
1. Initial database schema (`Schema.sql`).
2. Sample data (`SeedData.sql`).
3. CRUD operations in `Repositories`.

---

## Testing

### Database Queries
Test SQL queries directly in the PostgreSQL CLI or GUI tools like DBeaver.

### Application
Run the application and verify:
- Orders are saved correctly.
- Inventory levels are updated.
- Customer and order details are fetched without errors.

---
