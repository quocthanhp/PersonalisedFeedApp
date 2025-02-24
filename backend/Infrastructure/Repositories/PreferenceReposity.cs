using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories;

public class PreferenceRepository : IPreferenceRepository
{
    private AppDbContext _db;

    public PreferenceRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<Preference?> CreateAsync(Preference p)
    {
        if (_db.Preferences == null)
        {
            throw new InvalidOperationException("Preferences DbSet is null.");
        }

        EntityEntry<Preference> added = await _db.Preferences.AddAsync(p);
        int affected = await _db.SaveChangesAsync();
        if (affected == 1)
        {
            return p;
        }

        return null;
    }

    public async Task<bool?> DeleteAsync(string id)
    {
        if (_db.Preferences == null)
        {
            throw new InvalidOperationException("Preferences DbSet is null.");
        }

        Preference? p = await _db.Preferences.FindAsync(id);
        if (p is null) return null;

        _db.Preferences.Remove(p);
        int affected = await _db.SaveChangesAsync();
        if (affected == 1)
        {
            return true;
        }

        return null;
    }

    public Task<Preference[]> RetrieveAllAsync()
    {
        if (_db.Preferences == null)
        {
            throw new InvalidOperationException("Preferences DbSet is null.");
        }

        return _db.Preferences.ToArrayAsync();
    }

    public Task<Preference?> RetrieveAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Preference?> UpdateAsync(Preference p)
    {
        throw new NotImplementedException();
    }
}