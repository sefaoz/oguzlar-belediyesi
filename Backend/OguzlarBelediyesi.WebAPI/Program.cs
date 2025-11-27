using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OguzlarBelediyesi.Application;
using OguzlarBelediyesi.Application.Filters;
using OguzlarBelediyesi.Infrastructure;
using OguzlarBelediyesi.Infrastructure.Data;
using OguzlarBelediyesi.Infrastructure.Database;
using OguzlarBelediyesi.Infrastructure.Repositories;
using OguzlarBelediyesi.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

const string OguzlarCorsPolicy = "_oguzlarCors";

builder.Services.AddOpenApi();
builder.Services.AddSingleton<INewsRepository, NewsRepository>();
builder.Services.AddSingleton<IPageContentRepository, PageContentRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ITenderRepository, TenderRepository>();
builder.Services.AddDbContext<OguzlarBelediyesiDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ??
                      "Data Source=oguzlar.db"));
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
        policy.WithOrigins("http://localhost:4200")
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
.WithName("Login");

app.MapGet("/api/news", async (INewsRepository repository) =>
{
    var news = await repository.GetAllAsync();
    return Results.Ok(news);
})
.WithName("GetNews");

app.MapGet("/api/news/{slug}", async (string slug, INewsRepository repository) =>
{
    var newsItem = await repository.GetBySlugAsync(slug);
    return newsItem is not null ? Results.Ok(newsItem) : Results.NotFound();
})
.WithName("GetNewsBySlug");

app.MapGet("/api/pages/{key}", async (string key, IPageContentRepository repository) =>
{
    var pageContent = await repository.GetByKeyAsync(key);
    return pageContent is not null ? Results.Ok(pageContent) : Results.NotFound();
})
.WithName("GetPageContent");

app.MapGet("/api/announcements", async ([AsParameters] AnnouncementQuery query, IAnnouncementRepository repository) =>
{
    var filter = new AnnouncementFilter(query.search, query.from, query.to);
    var announcements = await repository.GetAllAsync(filter);
    return Results.Ok(announcements);
})
    .WithName("GetAnnouncements");

app.MapGet("/api/announcements/{slug}", async (string slug, IAnnouncementRepository repository) =>
{
    var announcement = await repository.GetBySlugAsync(slug);
    return announcement is not null ? Results.Ok(announcement) : Results.NotFound();
})
    .WithName("GetAnnouncementBySlug");

app.MapGet("/api/events", async ([AsParameters] EventQuery query, IEventRepository repository) =>
{
    var filter = new EventFilter(query.search, query.upcomingOnly);
    var events = await repository.GetAllAsync(filter);
    return Results.Ok(events);
})
    .WithName("GetEvents");

app.MapGet("/api/events/{slug}", async (string slug, IEventRepository repository) =>
{
    var eventItem = await repository.GetBySlugAsync(slug);
    return eventItem is not null ? Results.Ok(eventItem) : Results.NotFound();
})
    .WithName("GetEventBySlug");

app.MapGet("/api/tenders", async ([AsParameters] TenderQuery query, ITenderRepository repository) =>
{
    var filter = new TenderFilter(query.search, query.status);
    var tenders = await repository.GetAllAsync(filter);
    return Results.Ok(tenders);
})
    .WithName("GetTenders");

app.MapGet("/api/tenders/{slug}", async (string slug, ITenderRepository repository) =>
{
    var tender = await repository.GetBySlugAsync(slug);
    return tender is not null ? Results.Ok(tender) : Results.NotFound();
})
    .WithName("GetTenderBySlug");

app.Run();

record AnnouncementQuery(string? search, DateTime? from, DateTime? to);
record EventQuery(string? search, bool upcomingOnly = false);
record TenderQuery(string? search, string? status);
record LoginRequest(string Username, string Password);
