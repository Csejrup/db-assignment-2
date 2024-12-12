using Onlinebookstore.Models;
using Onlinebookstore.Repositories;

namespace Onlinebookstore.Services;

public class AuthorService(AuthorRepository authorRepository, RedisCacheService cacheService)
{
    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        // Attempt to retrieve cached data
        var cachedData = await cacheService.GetCacheAsync<List<Author>>("authors_all");
        if (cachedData != null)
        {
            return cachedData;
        }

        // Retrieve authors from repository
        var authors = await authorRepository.GetAllAuthorsAsync();

        // Cache the result if authors are retrieved
        if (authors.Any())
        {
            await cacheService.SetCacheAsync<List<Author>>("authors_all", authors, TimeSpan.FromMinutes(5));
        }

        return authors;
    }

    public async Task<List<Author>> GetAllAuthorsAsyncWithoutCache()
    {
        return await authorRepository.GetAllAuthorsAsync();
    }

    public async Task AddAuthorAsync(string name, string biography)
    {
        // Create a new Author object
        var author = new Author
        {
            Name = name,
            Biography = biography
        };

        // Add the author to the repository
        await authorRepository.AddAuthorAsync(author);

        // Clear the cache for the list of authors
        await cacheService.DeleteCacheAsync<List<Author>>("authors_all");
    }

    public async Task<Author?> GetAuthorByIdAsync(int authorId)
    {
        // Check if data exists in the cache
        var cachedData = await cacheService.GetCacheAsync<Author>(authorId.ToString());
        if (cachedData != null)
        {
            return cachedData;
        }

        // Retrieve the author from the repository
        var author = await authorRepository.GetAuthorByIdAsync(authorId);

        // Cache the author data if it exists
        if (author != null)
        {
            await cacheService.SetCacheAsync<Author>(authorId.ToString(), author, TimeSpan.FromHours(1));
        }

        return author;
    }

    public async Task UpdateAuthorBiographyAsync(int authorId, string newBiography)
    {
        // Clear the cache for the specific author
        await cacheService.DeleteCacheAsync<Author>(authorId.ToString());

        // Clear the cache for the list of authors
        await cacheService.DeleteCacheAsync<List<Author>>("authors_all");

        // Update the author's biography in the repository
        await authorRepository.UpdateAuthorBiographyAsync(authorId, newBiography);
    }

    public async Task DeleteAuthorAsync(int authorId)
    {
        // Clear the cache for the specific author
        await cacheService.DeleteCacheAsync<Author>(authorId.ToString());

        // Clear the cache for the list of authors
        await cacheService.DeleteCacheAsync<List<Author>>("authors_all");

        // Delete the author from the repository
        await authorRepository.DeleteAuthorAsync(authorId);
    }
}
