using dotnet_simplified_bank.Data;
using dotnet_simplified_bank.Interfaces;
using dotnet_simplified_bank.Models;
using dotnet_simplified_bank.Repositories;
using dotnet_simplified_bank.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(options => {})
    .AddEntityFrameworkStores<AppDatabaseContext>();

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme =
    opts.DefaultChallengeScheme =
    opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOpts =>
{
    jwtOpts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:LoginKey"]!))
    };
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITransferRepository, TransferRepository>();
builder.Services.AddHttpClient<IExternalServices, ExternalServices>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.43.3");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();

app.MapGet("/", () => "Simplified Bank API");

app.UseAuthorization();

app.MapControllers();

app.Run();
