namespace Onlinebookstore.Models;

public class Author
{
    public int AuthorID { get; set; }
    public string Name { get; set; }
    public string Biography { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();

}