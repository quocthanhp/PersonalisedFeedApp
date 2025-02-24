using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories;

public class NewsArticleRepository : INewsArticleRepository
{
    private AppDbContext _db;

    public NewsArticleRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<NewsArticle?> CreateAsync(NewsArticle n)
    {
        if (_db.NewsArticles == null)
        {
            throw new InvalidOperationException("NewsArticles DbSet is null.");
        }

        EntityEntry<NewsArticle> added = await _db.NewsArticles.AddAsync(n);
        int affected = await _db.SaveChangesAsync();

        if (affected == 1)
        {
            return n;
        }

        return null;
    }

    public Task<bool?> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<NewsArticle[]> RetrieveAllAsync()
    {
        if (_db.NewsArticles == null)
        {
            throw new InvalidOperationException("NewsArticles DbSet is null.");
        }

        return _db.NewsArticles.ToArrayAsync();
    }

    public Task<NewsArticle?> RetrieveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<NewsArticle?> UpdateAsync(NewsArticle n)
    {
        throw new NotImplementedException();
    }
}
