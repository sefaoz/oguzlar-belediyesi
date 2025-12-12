using System;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Contracts.Requests;
using OguzlarBelediyesi.WebAPI.Filters;
using OguzlarBelediyesi.WebAPI.Helpers;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/news")]
public sealed class NewsController : ControllerBase
{
    private readonly INewsRepository _repository;
    private readonly IWebHostEnvironment _env;

    public NewsController(INewsRepository repository, IWebHostEnvironment env)
    {
        _repository = repository;
        _env = env;
    }

    [HttpGet]
    [Cache(60, "News")]
    public async Task<ActionResult<IEnumerable<NewsItem>>> GetAll(CancellationToken cancellationToken)
    {
        var news = await _repository.GetAllAsync(cancellationToken);
        return Ok(news);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<NewsItem>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var newsItem = await _repository.GetBySlugAsync(slug, cancellationToken);
        
        if (newsItem is not null)
        {
             await _repository.IncrementViewCountAsync(slug, cancellationToken);
             await _repository.SaveChangesAsync(cancellationToken);
        }

        return newsItem is null ? NotFound() : Ok(newsItem);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("News")]
    public async Task<IActionResult> Create([FromForm] NewsRequest request, [FromForm] IFormFile? file, [FromForm] List<IFormFile>? galleryFiles, CancellationToken cancellationToken)
    {
        var image = request.Image ?? string.Empty;

        if (file is not null)
        {
            image = await ImageHelper.SaveImageAsWebPAsync(file, "uploads/news", _env.WebRootPath);
        }

        var photos = new List<string>();
        if (galleryFiles is not null && galleryFiles.Count > 0)
        {
             foreach (var f in galleryFiles)
             {
                 var photoPath = await ImageHelper.SaveImageAsWebPAsync(f, "uploads/news/gallery", _env.WebRootPath);
                 if (!string.IsNullOrEmpty(photoPath))
                 {
                    photos.Add(photoPath);
                 }
             }
        }

        var slug = await GenerateUniqueSlugAsync(request.Title, null, cancellationToken);

        var newsItem = new NewsItem
        {
            Id = Guid.NewGuid(),
            Image = image,
            Date = request.Date,
            Title = request.Title,
            Description = request.Description,
            Slug = slug,
            Photos = photos,
            Tags = request.Tags?.ToList()
        };

        await _repository.AddAsync(newsItem, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return Created($"/api/news/{newsItem.Slug}", newsItem);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("News")]
    public async Task<IActionResult> Update(Guid id, [FromForm] NewsRequest request, [FromForm] IFormFile? file, [FromForm] List<IFormFile>? galleryFiles, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

        var image = existing.Image;

        if (file is not null)
        {
            image = await ImageHelper.SaveImageAsWebPAsync(file, "uploads/news", _env.WebRootPath);
        }

        else if (!string.IsNullOrEmpty(request.Image))
        {
            image = request.Image;
        }

        var currentPhotos = request.Photos?.ToList() ?? new List<string>();
        if (galleryFiles is not null && galleryFiles.Count > 0)
        {
             foreach (var f in galleryFiles)
             {
                 var photoPath = await ImageHelper.SaveImageAsWebPAsync(f, "uploads/news/gallery", _env.WebRootPath);
                 if (!string.IsNullOrEmpty(photoPath))
                 {
                    currentPhotos.Add(photoPath);
                 }
             }
        }

        var slug = existing.Slug;
        if (existing.Title != request.Title)
        {
            slug = await GenerateUniqueSlugAsync(request.Title, id, cancellationToken);
        }

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.Date = request.Date;
        existing.Slug = slug;
        existing.Image = image;
        existing.Photos = currentPhotos;
        existing.Tags = request.Tags?.ToList();

        await _repository.UpdateAsync(existing, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("News")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, Guid? excludeId, CancellationToken cancellationToken)
    {
        var baseSlug = SlugHelper.GenerateSlug(title);
        var slug = baseSlug;
        var counter = 2;

        while (await _repository.SlugExistsAsync(slug, excludeId, cancellationToken))
        {
            slug = SlugHelper.AppendNumber(baseSlug, counter);
            counter++;
        }

        return slug;
    }
}
