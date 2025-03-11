using DotNetEnv;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Application.Interfaces;
using Infrastructure.Services;
using Infrastructure.Messaging;
using Swashbuckle.AspNetCore.SwaggerUI;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Load the .env file into environment variables
Env.Load("/Users/quocthanhpham/Documents/PersonalisedFeedApp/backend/.env");

// Access the connection string from the environment variables
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

Console.WriteLine($"Connection String: {connectionString}");

// Add DbContext with the connection string from .env file
builder.Services.AddAppDbContext(connectionString);

// Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add authorization
builder.Services.AddAuthorization();

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

builder.Services.AddSingleton<IMessagePublisher, Publisher>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
