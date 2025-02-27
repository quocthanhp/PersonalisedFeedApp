using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace UnitTests;

public class DbTest
{
    [Fact]
    public void DatabaseConnectTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        using var dbContext = new AppDbContext(options);
        bool canConnect = dbContext.Database.CanConnect();

        Assert.True(canConnect);
    }
}