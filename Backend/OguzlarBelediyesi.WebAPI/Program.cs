using System;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Application.Contracts.Services;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Logging;
using OguzlarBelediyesi.Infrastructure.Persistence.Data;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;
using OguzlarBelediyesi.Infrastructure.Persistence.Repositories;
using OguzlarBelediyesi.Infrastructure.Security;
using OguzlarBelediyesi.WebAPI.Filters;
using OguzlarBelediyesi.WebAPI.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel for large file uploads (500MB max)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 500 * 1024 * 1024; // 500MB
});

// Configure Form options for multipart uploads
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 500 * 1024 * 1024; // 500MB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

const string OguzlarCorsPolicy = "_oguzlarCors";
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "OguzlarBelediyesi")
        .WriteTo.Console()
        .WriteTo.Sink(new MySqlLogSink(connectionString, "SerilogLogs"));
});

await EnsureDatabaseExistsAsync(connectionString);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<ActionLogFilter>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<CacheResultFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<ActionLogFilter>();
    options.Filters.AddService<CacheResultFilter>();
});
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IPageContentRepository, PageContentRepository>();
builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
builder.Services.AddScoped<ICouncilRepository, CouncilRepository>();
builder.Services.AddScoped<IKvkkRepository, KvkkRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IMunicipalUnitRepository, MunicipalUnitRepository>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ITenderRepository, TenderRepository>();
builder.Services.AddScoped<ISiteSettingsRepository, SiteSettingsRepository>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<OguzlarBelediyesiDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions =>
    {
        mySqlOptions.EnableRetryOnFailure();
        mySqlOptions.EnableStringComparisonTranslations();
    }));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var jwtSection = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection.Issuer,
            ValidAudience = jwtSection.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy(OguzlarCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4201")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Rate Limiting yapılandırması - Brute-force saldırılarına karşı koruma
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    
    // Genel API limiti: IP başına dakikada 100 istek
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 5;
    });
    
    // Login endpoint için özel sıkı limit: IP başına dakikada 5 deneme
    options.AddFixedWindowLimiter("auth", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });
    
    // Global fallback policy
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 200,
                Window = TimeSpan.FromMinutes(1)
            }));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(OguzlarCorsPolicy);
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task EnsureDatabaseExistsAsync(string connectionString)
{
    var builder = new MySqlConnectionStringBuilder(connectionString);
    var databaseName = builder.Database;
    if (string.IsNullOrWhiteSpace(databaseName))
    {
        throw new InvalidOperationException("Connection string must specify a database.");
    }

    builder.Database = string.Empty;
    using var connection = new MySqlConnection(builder.ConnectionString);
    await connection.OpenAsync();

    using var command = connection.CreateCommand();
    command.CommandText = $"CREATE DATABASE IF NOT EXISTS `{databaseName}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
    await command.ExecuteNonQueryAsync();
}
