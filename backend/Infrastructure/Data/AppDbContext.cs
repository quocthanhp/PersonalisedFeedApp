using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Preference> Preferences { set; get; }
    public DbSet<ContentItem> ContentItems { set; get; }

    //public DbSet<UserContentItem> UserContentItems { set; get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Each User can have multiple preferences
        // modelBuilder.Entity<User>()
        //     .HasMany(u => u.Preferences)
        //     .WithOne(p => p.User)
        //     .HasForeignKey(p => p.UserId)
        //     .OnDelete(DeleteBehavior.Cascade);  // Cascade delete user preferences if user is deleted

        modelBuilder.Entity<ContentItem>()
            .HasDiscriminator<string>("ContentType")
            .HasValue<NewsArticle>("NewsArticle");

        // modelBuilder.Entity<UserContentItem>()
        //     .HasKey(uci => new { uci.UserId, uci.ContentItemId });

        // modelBuilder.Entity<UserContentItem>()
        //     .HasOne(uci => uci.User)
        //     .WithMany(u => u.UserContentItems)
        //     .HasForeignKey(una => una.UserId)
        //     .OnDelete(DeleteBehavior.Cascade);  // Cascade delete users' associations

        // modelBuilder.Entity<UserContentItem>()
        //     .HasOne(uci => uci.ContentItem)
        //     .WithMany(ci => ci.UserContentItems)
        //     .HasForeignKey(uci => uci.ContentItemId)
        //     .OnDelete(DeleteBehavior.Cascade);  // Cascade delete articles' associations
    }
}