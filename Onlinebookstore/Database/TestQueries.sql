
-- Indexing for Foreign Keys
CREATE INDEX idx_books_authorid ON books(authorid);
CREATE INDEX idx_orders_customerid ON orders(customerid);
CREATE INDEX idx_orderdetails_orderid ON orderdetails(orderid);
CREATE INDEX idx_orderdetails_bookid ON orderdetails(bookid);

-- Indexing for Frequently Filtered or Sorted Columns
CREATE INDEX idx_books_stock ON books(stock);
CREATE INDEX idx_orders_orderdate ON orders(orderdate);

-- Query 1: Retrieve All Books with Authors
-- Optimized with indexing on books.authorid and authors.authorid
SELECT b.bookid, b.title, b.price, b.stock, a.name AS author_name
FROM books b
         JOIN authors a ON b.authorid = a.authorid;

-- Query 2: Retrieve All Orders with Details
-- Optimized with indexing on orders.customerid and customers.customerid
SELECT o.orderid, o.orderdate, o.totalamount, c.name AS customer_name, c.email
FROM orders o
         JOIN customers c ON o.customerid = c.customerid;

-- Query 3: Retrieve All Order Details for an Order
-- Optimized with indexing on orderdetails.orderid and orderdetails.bookid
SELECT od.orderid, od.quantity, od.price, b.title
FROM orderdetails od
         JOIN books b ON od.bookid = b.bookid
WHERE od.orderid = 1; -- Replace with a valid OrderID

-- Query 4: Retrieve Total Stock of Books
-- Optimized with indexing on books.stock
SELECT SUM(stock) AS total_stock FROM books;

-- Query 5: Add a New Order
INSERT INTO orders (customerid, orderdate, totalamount)
VALUES (1, '2024-12-12 10:30:00', 59.97)
    RETURNING orderid;

-- Use the returned OrderID to insert order details
INSERT INTO orderdetails (orderid, bookid, quantity, price)
VALUES (3, 1, 3, 19.99);

-- Query 6: Update Stock When an Order is Placed
-- Optimized with indexing on books.bookid
UPDATE books
SET stock = stock - 3
WHERE bookid = 1;

-- Query 7: Retrieve Customers with Multiple Orders
-- Optimized with indexing on customers.customerid and orders.customerid
SELECT c.customerid, c.name, COUNT(o.orderid) AS order_count
FROM customers c
         JOIN orders o ON c.customerid = o.customerid
GROUP BY c.customerid, c.name
HAVING COUNT(o.orderid) > 1;

-- Query 8: Retrieve All Books with Their Order Counts
-- Optimized with indexing on books.bookid and orderdetails.bookid
SELECT b.bookid, b.title, COUNT(od.orderdetailid) AS order_count
FROM books b
         LEFT JOIN orderdetails od ON b.bookid = od.bookid
GROUP BY b.bookid, b.title;

-- Query 9: Retrieve All Orders with Total Quantity and Total Price
-- Optimized with indexing on orders.orderid and orderdetails.orderid
SELECT o.orderid, o.orderdate, SUM(od.quantity) AS total_quantity, SUM(od.price) AS total_price
FROM orders o
         JOIN orderdetails od ON o.orderid = od.orderid
GROUP BY o.orderid, o.orderdate;

-- Query 10: Check Stock for Books Below a Threshold
-- Optimized with indexing on books.stock
SELECT bookid, title, stock
FROM books
WHERE stock < 10; -- Adjust the threshold as needed

-- Query 11: Delete an Order and Check Cascading Behavior
-- Verify cascading delete works as intended
DELETE FROM orders WHERE orderid = 1;

-- Verify the order and related details are removed
SELECT * FROM orders WHERE orderid = 1;
SELECT * FROM orderdetails WHERE orderid = 1;
