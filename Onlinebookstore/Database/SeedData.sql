
-- SeedData.sql: Sample Data for Online Bookstore

-- Authors Data
INSERT INTO Authors (Name, Biography) VALUES
    ('J.K. Rowling', 'Author of the Harry Potter series'),
    ('George R.R. Martin', 'Author of A Song of Ice and Fire');

-- Books Data
INSERT INTO Books (Title, AuthorID, Price, Stock) VALUES
    ('Harry Potter and the Philosopher''s Stone', 1, 19.99, 100),
    ('A Game of Thrones', 2, 29.99, 50);

-- Customers Data
INSERT INTO Customers (Name, Email, Address) VALUES
    ('John Doe', 'john.doe@example.com', '123 Elm Street'),
    ('Jane Smith', 'jane.smith@example.com', '456 Maple Avenue');

-- Orders Data
INSERT INTO Orders (CustomerID, OrderDate, TotalAmount) VALUES
    (1, '2024-12-10 10:00:00', 49.98),
    (2, '2024-12-11 14:30:00', 29.99);

-- OrderDetails Data
INSERT INTO OrderDetails (OrderID, BookID, Quantity, Price) VALUES
    (1, 1, 2, 19.99),
    (2, 2, 1, 29.99);
