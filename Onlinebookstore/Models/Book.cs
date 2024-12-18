namespace Onlinebookstore.Models;

public class Book
{
    public int BookID { get; set; }
    public string Title { get; set; }
    public int AuthorID { get; set; }
    public Author Author { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

}