using Domain.Entities;

namespace Application.Interfaces;

public interface IPreferenceRepository
{
    Task<Preference?> CreateAsync(Preference p);
    Task<Preference[]> RetrieveAllAsync();
    Task<Preference?> RetrieveAsync(int id);
    Task<Preference?> UpdateAsync(Preference p);
    Task<bool?> DeleteAsync(string id);
}