using Microsoft.EntityFrameworkCore;
using Onlinebookstore.Models;

namespace Onlinebookstore.Repositories;

public class AuthorRepository(AppDbContext context)
{
  // Fetch all authors with their books
  public async Task<List<Author>> GetAllAuthorsAsync()
  {
    return await context.Authors.Include(a => a.Books).ToListAsync();
  }

  // Add a new author
  public async Task AddAuthorAsync(Author author)
  {
    context.Authors.Add(author);
    await context.SaveChangesAsync();
  }

  // Fetch an author by their ID, including their books
  public async Task<Author?> GetAuthorByIdAsync(int authorId)
  {
    return await context.Authors
      .Include(a => a.Books)
      .FirstOrDefaultAsync(a => a.AuthorID == authorId);
  }

  // Update an author's biography
  public async Task UpdateAuthorBiographyAsync(int authorId, string biography)
  {
    var author = await context.Authors.FindAsync(authorId);
    if (author != null)
    {
      author.Biography = biography;
      await context.SaveChangesAsync();
    }
  }

  // Delete an author
  public async Task DeleteAuthorAsync(int authorId)
  {
    var author = await context.Authors.FindAsync(authorId);
    if (author != null)
    {
      context.Authors.Remove(author);
      await context.SaveChangesAsync();
    }
  }
}