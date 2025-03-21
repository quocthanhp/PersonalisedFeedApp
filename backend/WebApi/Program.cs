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
using Microsoft.OpenApi.Models;


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
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])
        )
    };
});

// Add authorization
// builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Feed Service API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });


builder.Services.AddScoped<IPreferenceRepository, PreferenceRepository>();
builder.Services.AddScoped<IContentItemRepository, ContentItemRepository>();
builder.Services.AddScoped<IFetchNewsService, FetchNewsService>();
builder.Services.AddHttpClient<IFetchNewsService, FetchNewsService>();
builder.Services.AddScoped<IFetchRedditPostService, FetchRedditPostService>();
builder.Services.AddHttpClient<IFetchRedditPostService, FetchRedditPostService>();
builder.Services.AddSingleton<IMessagePublisher, Publisher>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddLogging(logging =>
{
    logging.AddConsole();  // Enables logging to the console
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    //.WithOrigins("http://localhost:3000") // used when deploy (set to domain)
    .SetIsOriginAllowed(origin => true)
    );


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
