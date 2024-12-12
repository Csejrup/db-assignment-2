namespace Onlinebookstore.Models;

public class Customer
{
    public int CustomerID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();

}