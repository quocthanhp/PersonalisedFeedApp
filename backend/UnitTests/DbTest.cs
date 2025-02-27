using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace UnitTests;

public class DbTest
{
    private async Task<AppDbContext> GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        var dbContext = new AppDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();

        return dbContext;
    }

    [Fact]
    public async Task DatabaseConnectTest()
    {
        using var dbContext = await GetDbContext();
        bool canConnect = dbContext.Database.CanConnect();

        Assert.True(canConnect);
    }

    [Fact]
    public async Task AddArticleTest()
    {
        using var dbContext = await GetDbContext();
        var repo = new ContentItemRepository(dbContext);

        var article = new NewsArticle
        {
            Source = "Test Source",
            Author = "Test Author",
            Title = "Test Title",
            PublishedAt = DateTime.Now,
            Content = "Test Content",
            Topic = "Test Topic"
        };

        var added = await repo.CreateAsync(article) as NewsArticle;

        Assert.NotNull(added);
        Assert.Equal(article.Source, added?.Source);
        Assert.Equal(article.Author, added?.Author);
        Assert.Equal(article.Title, added?.Title);
        Assert.Equal(article.PublishedAt, added?.PublishedAt);
    }

    [Fact]
    public async Task RetrieveAllArticlesTest()
    {
        using var dbContext = await GetDbContext();
        var repo = new ContentItemRepository(dbContext);

        for (int i = 0; i < 10; i++)
        {
            var article = new NewsArticle
            {
                Source = "Test Source",
                Author = "Test Author",
                Title = "Test Title",
                PublishedAt = DateTime.Now,
                Content = "Test Content",
                Topic = "Test Topic",
                UserContentItems = new List<UserContentItem>
                {
                    new UserContentItem
                    {
                        UserId = 1,
                    }
                }
            };

            await repo.CreateAsync(article);
        }

        var articles = await repo.RetrieveAllAsync("1");

        Assert.NotNull(articles);
        Assert.Equal(10, articles.Length);
    }

    [Fact]
    public async Task CreatePreferenceTest()
    {
        using var dbContext = await GetDbContext();
        var repo = new PreferenceRepository(dbContext);

        var preference = new Preference
        {
            UserId = 1,
            Topic = "Test Topic"
        };

        var added = await repo.CreateAsync(preference);

        Assert.NotNull(added);
        Assert.Equal(preference.UserId, added?.UserId);
        Assert.Equal(preference.Topic, added?.Topic);
    }
}