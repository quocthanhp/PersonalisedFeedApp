using DotNetEnv;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Application.Interfaces;
using Infrastructure.Services;




var builder = WebApplication.CreateBuilder(args);

// Load the .env file into environment variables
Env.Load();

// Access the connection string from the environment variables
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
