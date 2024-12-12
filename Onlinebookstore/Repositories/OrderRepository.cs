using Microsoft.EntityFrameworkCore;
using Onlinebookstore.Models;

namespace Onlinebookstore.Repositories;

public class OrderRepository(AppDbContext context)
{
   
    public async Task CreateOrderAsync(int customerId, decimal totalAmount, List<OrderDetail> orderDetails)
    {
        var order = new Order
        {
            CustomerID = customerId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = totalAmount,
            OrderDetails = orderDetails
        };

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
    }

    public async Task<List<Order>> GetCustomerOrdersAsync(int customerId)
    {
        return await context.Orders
            .Include(o => o.OrderDetails) 
            .ThenInclude(od => od.Book)  
            .Where(o => o.CustomerID == customerId)
            .ToListAsync();
    }

}