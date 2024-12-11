
-- Schema.sql: Database Schema for Online Bookstore

-- Authors Table
CREATE TABLE Authors (
    AuthorID SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Biography TEXT
);

-- Books Table
CREATE TABLE Books (
    BookID SERIAL PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    AuthorID INT REFERENCES Authors(AuthorID),
    Price DECIMAL(10, 2) NOT NULL,
    Stock INT NOT NULL
);

-- Customers Table
CREATE TABLE Customers (
    CustomerID SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Address TEXT
);

-- Orders Table
CREATE TABLE Orders (
    OrderID SERIAL PRIMARY KEY,
    CustomerID INT REFERENCES Customers(CustomerID),
    OrderDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    TotalAmount DECIMAL(10, 2) NOT NULL
);

-- OrderDetails Table
CREATE TABLE OrderDetails (
    OrderDetailID SERIAL PRIMARY KEY,
    OrderID INT REFERENCES Orders(OrderID),
    BookID INT REFERENCES Books(BookID),
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);
