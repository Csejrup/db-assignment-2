-- Authors Data
INSERT INTO authors (name, biography) VALUES
    ('J.K. Rowling', 'Author of the Harry Potter series'),
    ('George R.R. Martin', 'Author of A Song of Ice and Fire');

-- Books Data
INSERT INTO books (title, authorid, price, stock) VALUES
    ('Harry Potter and the Philosopher''s Stone', 1, 19.99, 100),
    ('A Game of Thrones', 2, 29.99, 50),
    ('Harry Potter and the Chamber of Secrets', 1, 18.99, 75);

-- Customers Data
INSERT INTO customers (name, email, address) VALUES
    ('John Doe', 'john.doe@example.com', '123 Elm Street'),
    ('Jane Smith', 'jane.smith@example.com', '456 Maple Avenue');

-- Orders Data
INSERT INTO orders (customerid, orderdate, totalamount) VALUES
    (1, '2024-12-10 10:00:00', 59.97),
    (2, '2024-12-11 14:30:00', 29.99);

-- OrderDetails Data
INSERT INTO orderdetails (orderid, bookid, quantity, price) VALUES
    (1, 1, 2, 19.99),
    (1, 3, 1, 18.99),
    (2, 2, 1, 29.99);
