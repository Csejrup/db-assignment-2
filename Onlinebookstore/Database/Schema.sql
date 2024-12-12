-- Authors Table
CREATE TABLE authors (
    authorid SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    biography TEXT
);

-- Books Table
CREATE TABLE books (
    bookid SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    authorid INT REFERENCES authors(authorid),
    price DECIMAL(10, 2) NOT NULL,
    stock INT NOT NULL
);

-- Customers Table
CREATE TABLE customers (
    customerid SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    address TEXT
);

-- Orders Table
CREATE TABLE orders (
    orderid SERIAL PRIMARY KEY,
    customerid INT REFERENCES customers(customerid),
    orderdate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    totalamount DECIMAL(10, 2) NOT NULL
);

-- OrderDetails Table
CREATE TABLE orderdetails (
    orderdetailid SERIAL PRIMARY KEY,
    orderid INT REFERENCES orders(orderid) ON DELETE CASCADE,
    bookid INT REFERENCES books(bookid),
    quantity INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL
);

-- Indexes for Optimization
CREATE INDEX idx_books_authorid ON books(authorid);
CREATE INDEX idx_orders_customerid ON orders(customerid);
CREATE INDEX idx_orderdetails_orderid ON orderdetails(orderid);
CREATE INDEX idx_orderdetails_bookid ON orderdetails(bookid);
CREATE INDEX idx_books_stock ON books(stock);
CREATE INDEX idx_orders_orderdate ON orders(orderdate);
