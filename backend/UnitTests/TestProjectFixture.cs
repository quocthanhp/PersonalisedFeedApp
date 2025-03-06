using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Infrastructure.Services;
using Xunit.Microsoft.DependencyInjection;

namespace UnitTests
{
    public class TestProjectFixture : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
        => services.AddTransient<IFetchRedditPostService, FetchRedditPostService>();

        protected override ValueTask DisposeAsyncCore()
        => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new() { Filename = "appsettings.json", IsOptional = true };
        }
    }
}