using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Application.Contracts.Services;
using OguzlarBelediyesi.Application.Filters;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Logging;
using OguzlarBelediyesi.Infrastructure.Persistence.Data;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;
using OguzlarBelediyesi.Infrastructure.Persistence.Repositories;
using OguzlarBelediyesi.Infrastructure.Security;
using OguzlarBelediyesi.WebAPI.Filters;
using OguzlarBelediyesi.WebAPI;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IPageContentRepository, PageContentRepository>();
builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
builder.Services.AddScoped<ICouncilRepository, CouncilRepository>();
builder.Services.AddScoped<IKvkkRepository, KvkkRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IMunicipalUnitRepository, MunicipalUnitRepository>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ITenderRepository, TenderRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<AspectCacheFilter>();
builder.Services.AddScoped<AspectLogFilter>();
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
app.UseCors(OguzlarCorsPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/auth/login", async (LoginRequest request, IAuthenticationService authenticationService) =>
{
    var token = await authenticationService.AuthenticateAsync(request.Username, request.Password);
    return token is not null ? Results.Ok(new { token }) : Results.Unauthorized();
})
    .WithName("Login")
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/news", async (INewsRepository repository) =>
{
    var news = await repository.GetAllAsync();
    return Results.Ok(news);
})
    .WithName("GetNews")
    .WithMetadata(new CacheAttribute(60))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/news/{slug}", async (string slug, INewsRepository repository) =>
{
    var newsItem = await repository.GetBySlugAsync(slug);
    return newsItem is not null ? Results.Ok(newsItem) : Results.NotFound();
})
    .WithName("GetNewsBySlug")
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/pages/{key}", async (string key, IPageContentRepository repository) =>
{
    var pageContent = await repository.GetByKeyAsync(key);
    return pageContent is not null ? Results.Ok(pageContent) : Results.NotFound();
})
    .WithName("GetPageContent")
    .WithMetadata(new CacheAttribute(120))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/meclis", async (ICouncilRepository repository) =>
{
    var documents = await repository.GetAllAsync();
    return Results.Ok(documents);
})
    .WithName("GetCouncilDocuments")
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/gallery/folders", async (IGalleryRepository repository) =>
{
    var folders = await repository.GetFoldersAsync();
    return Results.Ok(folders);
})
    .WithName("GetGalleryFolders")
    .WithMetadata(new CacheAttribute(120))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/gallery/folders/{folderId}", async (string folderId, IGalleryRepository repository) =>
{
    var folder = await repository.GetFolderByIdAsync(folderId);
    return folder is not null ? Results.Ok(folder) : Results.NotFound();
})
    .WithName("GetGalleryFolder")
    .WithMetadata(new CacheAttribute(120))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/gallery/folders/slug/{slug}", async (string slug, IGalleryRepository repository) =>
{
    var folder = await repository.GetFolderBySlugAsync(slug);
    return folder is not null ? Results.Ok(folder) : Results.NotFound();
})
    .WithName("GetGalleryFolderBySlug")
    .WithMetadata(new CacheAttribute(120))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/gallery/folders/{folderId}/images", async (string folderId, IGalleryRepository repository) =>
{
    var images = await repository.GetImagesByFolderAsync(folderId);
    return Results.Ok(images);
})
    .WithName("GetGalleryImages")
    .WithMetadata(new CacheAttribute(120))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/kvkk", async (IKvkkRepository repository) =>
{
    var documents = await repository.GetAllAsync();
    return Results.Ok(documents);
})
    .WithName("GetKvkkDocuments")
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/vehicles", async (IVehicleRepository repository) =>
{
    var vehicles = await repository.GetAllAsync();
    return Results.Ok(vehicles);
})
    .WithName("GetVehicles")
    .WithMetadata(new CacheAttribute(120))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/sliders", async (ISliderRepository repository) =>
{
    var sliders = await repository.GetAllAsync();
    return Results.Ok(sliders);
})
    .WithName("GetSliders")
    .WithMetadata(new CacheAttribute(60))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapPost("/api/sliders", async (SliderRequest request, ISliderRepository repository) =>
{
    var slider = new Slider(Guid.NewGuid().ToString(), request.Title, request.Description, request.ImageUrl, request.Link, request.Order, request.IsActive);
    await repository.AddAsync(slider);
    await repository.SaveChangesAsync();
    return Results.Created($"/api/sliders/{slider.Id}", slider);
})
    .WithName("CreateSlider")
    .AddEndpointFilter<AspectLogFilter>();

app.MapPut("/api/sliders/{id}", async (string id, SliderRequest request, ISliderRepository repository) =>
{
    var existing = await repository.GetByIdAsync(id);
    if (existing is null)
    {
        return Results.NotFound();
    }

    var updated = existing with
    {
        Title = request.Title,
        Description = request.Description,
        ImageUrl = request.ImageUrl,
        Link = request.Link,
        Order = request.Order,
        IsActive = request.IsActive
    };

    await repository.UpdateAsync(updated);
    await repository.SaveChangesAsync();
    return Results.NoContent();
})
    .WithName("UpdateSlider")
    .AddEndpointFilter<AspectLogFilter>();

app.MapDelete("/api/sliders/{id}", async (string id, ISliderRepository repository) =>
{
    await repository.DeleteAsync(id);
    await repository.SaveChangesAsync();
    return Results.NoContent();
})
    .WithName("DeleteSlider")
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/units", async (IMunicipalUnitRepository repository) =>
{
    var units = await repository.GetAllAsync();
    return Results.Ok(units);
})
    .WithName("GetMunicipalUnits")
    .WithMetadata(new CacheAttribute(120))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/announcements", async ([AsParameters] AnnouncementQuery query, IAnnouncementRepository repository) =>
{
    var filter = new AnnouncementFilter(query.search, query.from, query.to);
    var announcements = await repository.GetAllAsync(filter);
    return Results.Ok(announcements);
})
    .WithName("GetAnnouncements")
    .WithMetadata(new CacheAttribute(60))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/announcements/{slug}", async (string slug, IAnnouncementRepository repository) =>
{
    var announcement = await repository.GetBySlugAsync(slug);
    return announcement is not null ? Results.Ok(announcement) : Results.NotFound();
})
    .WithName("GetAnnouncementBySlug")
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/events", async ([AsParameters] EventQuery query, IEventRepository repository) =>
{
    var filter = new EventFilter(query.search, query.upcomingOnly);
    var events = await repository.GetAllAsync(filter);
    return Results.Ok(events);
})
    .WithName("GetEvents")
    .WithMetadata(new CacheAttribute(60))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/events/{slug}", async (string slug, IEventRepository repository) =>
{
    var eventItem = await repository.GetBySlugAsync(slug);
    return eventItem is not null ? Results.Ok(eventItem) : Results.NotFound();
})
    .WithName("GetEventBySlug")
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/tenders", async ([AsParameters] TenderQuery query, ITenderRepository repository) =>
{
    var filter = new TenderFilter(query.search, query.status);
    var tenders = await repository.GetAllAsync(filter);
    return Results.Ok(tenders);
})
    .WithName("GetTenders")
    .WithMetadata(new CacheAttribute(60))
    .AddEndpointFilter<AspectCacheFilter>()
    .AddEndpointFilter<AspectLogFilter>();

app.MapGet("/api/tenders/{slug}", async (string slug, ITenderRepository repository) =>
{
    var tender = await repository.GetBySlugAsync(slug);
    return tender is not null ? Results.Ok(tender) : Results.NotFound();
})
    .WithName("GetTenderBySlug")
    .AddEndpointFilter<AspectLogFilter>();

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
