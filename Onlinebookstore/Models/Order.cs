namespace Onlinebookstore.Models;

public class Order
{
    public int OrderID { get; set; }
    public int CustomerID { get; set; }
    public Customer Customer { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

}