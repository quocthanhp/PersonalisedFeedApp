using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories;

public class ContentItemRepository : IContentItemRepository
{
    private AppDbContext _db;

    public ContentItemRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<ContentItem?> CreateAsync(ContentItem c)
    {
        EntityEntry<ContentItem> added = await _db.ContentItems.AddAsync(c);
        int affected = await _db.SaveChangesAsync();

        if (affected == 1)
        {
            return c;
        }

        return null;
    }

    public Task<bool?> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ContentItem[]> RetrieveAllAsync()
    {
        return _db.ContentItems.ToArrayAsync();
    }

    public Task<ContentItem?> RetrieveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ContentItem?> UpdateAsync(ContentItem c)
    {
        throw new NotImplementedException();
    }
}
