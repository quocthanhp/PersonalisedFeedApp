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

    public async Task<ContentItem[]> RetrieveAllAsync(string userId)
    {
        if (!int.TryParse(userId, out int userIdInt))
        {
            throw new ArgumentException("Invalid user ID format", nameof(userId));
        }

        return await (_db.ContentItems
                        .Where(i => i.UserContentItems.Any(uci => uci.UserId == userIdInt))
                        .ToArrayAsync());
    }

    public Task<ContentItem?> RetrieveAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ContentItem?> UpdateAsync(ContentItem c)
    {
        throw new NotImplementedException();
    }
}
