using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Application.Interfaces;
using DotNetEnv;
using Infrastructure.Data;

[assembly: FunctionsStartup(typeof(AzureFunctions.Startup))]
namespace AzureFunctions
{

    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<IFetchNewsService, FetchNewsService>();
            builder.Services.AddScoped<IContentItemRepository, ContentItemRepository>();

            // Load the .env file into environment variables
            Env.Load("/Users/quocthanhpham/Documents/PersonalisedFeedApp/backend/.env");

            // Access the connection string from the environment variables
            var connectionString = System.Environment.GetEnvironmentVariable("CONNECTION_STRING");

            // Add DbContext with the connection string from .env file
            builder.Services.AddAppDbContext(connectionString);
        }
    }
}