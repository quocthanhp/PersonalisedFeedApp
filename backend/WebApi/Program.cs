using DotNetEnv;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Application.Interfaces;
using Infrastructure.Services;
using Infrastructure.Messaging;
using Swashbuckle.AspNetCore.SwaggerUI;


var builder = WebApplication.CreateBuilder(args);

// Load the .env file into environment variables
Env.Load("/Users/quocthanhpham/Documents/PersonalisedFeedApp/backend/.env");

// Access the connection string from the environment variables
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

Console.WriteLine($"Connection String: {connectionString}");

// Add DbContext with the connection string from .env file
builder.Services.AddAppDbContext(connectionString);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPreferenceRepository, PreferenceRepository>();
builder.Services.AddScoped<IContentItemRepository, ContentItemRepository>();
builder.Services.AddScoped<IFetchNewsService, FetchNewsService>();
builder.Services.AddHttpClient<IFetchNewsService, FetchNewsService>();
builder.Services.AddScoped<IFetchRedditPostService, FetchRedditPostService>();
builder.Services.AddHttpClient<IFetchRedditPostService, FetchRedditPostService>();


// Set up queue
// Get RabbitMQ host from config (or use "localhost" as default)
string rabbitMqHost = builder.Configuration.GetValue<string>("RabbitMQ:Host") ?? "localhost";

// Register Publisher with the configured host
builder.Services.AddSingleton<IMessagePublisher>(sp => new Publisher(rabbitMqHost));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json",
            "Feed Service API Version 1");

        c.SupportedSubmitMethods(new[] {
            SubmitMethod.Get, SubmitMethod.Post,
            SubmitMethod.Put, SubmitMethod.Delete });
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
