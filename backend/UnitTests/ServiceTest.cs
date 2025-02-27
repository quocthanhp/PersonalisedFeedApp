using Infrastructure.Services;
using Xunit.Abstractions;

namespace UnitTests;

public class ServiceTest
{
    private readonly ITestOutputHelper _output;

    public ServiceTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async void FetchNewsServiceTest()
    {
        var fetchNewsService = new FetchNewsService(new System.Net.Http.HttpClient());

        var news = await fetchNewsService.FetchNewsAsync("AI");

        Assert.NotNull(news);
        //Assert.True(news.TotalResults > 0);
        //Assert.True(news.Articles.Count > 0);
        
        _output.WriteLine(news != null ? news.ToString() : "No news fetched");
    }
}
