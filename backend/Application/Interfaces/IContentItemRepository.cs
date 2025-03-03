using Domain.Entities;

namespace Application.Interfaces;

public interface IContentItemRepository
{
    Task<ContentItem?> CreateAsync(ContentItem c);
    Task<ContentItem[]> RetrieveAllAsync(string userId);
    Task<ContentItem?> RetrieveAsync(string id);
    Task<ContentItem?> UpdateAsync(ContentItem c);
    Task<bool?> DeleteAsync(string id);
    Task<IEnumerable<ContentItem>?> CreateBulkAsync(IEnumerable<ContentItem> contentItems); 
}