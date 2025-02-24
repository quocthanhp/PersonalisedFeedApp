using Domain.Entities;

namespace Application.Interfaces;

public interface IContentItemRepository
{
    Task<ContentItem?> CreateAsync(ContentItem c);
    Task<ContentItem[]> RetrieveAllAsync();
    Task<ContentItem?> RetrieveAsync(int id);
    Task<ContentItem?> UpdateAsync(ContentItem c);
    Task<bool?> DeleteAsync(string id);
}