using dotnet_simple_bank.Data;
using dotnet_simple_bank.Interfaces;
using dotnet_simple_bank.Models;
using dotnet_simple_bank.Repositories;
using dotnet_simple_bank.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using dotnet_simple_bank.Dtos;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(options => { })
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
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
    };
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITransferRepository, TransferRepository>();
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
builder.Services.AddHttpClient<IExternalServices, ExternalServices>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.43.3");
});

var app = builder.Build();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDatabaseContext>();

    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    var retry = Policy.Handle<Exception>().
        WaitAndRetry(
            retryCount: 5,
            sleepDurationProvider: attempts => TimeSpan.FromSeconds(Math.Pow(2, attempts)),
            onRetry: (exception, timeSpan) =>
            {
                logger.LogWarning(
                    "[RETRY] Failure applying database migrations. Retrying in {timeSpan.TotalSeconds} seconds. Error: {exception.Message}",
                    timeSpan.TotalSeconds, exception.Message
                );
            }
        );

    retry.Execute(() => { dbContext.Database.Migrate(); });
}

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();

// app.UseHttpsRedirection();

app.MapGet("/", () => new RootRoute(StatusCodes.Status200OK));

app.UseAuthorization();

app.MapControllers();

app.Run();
