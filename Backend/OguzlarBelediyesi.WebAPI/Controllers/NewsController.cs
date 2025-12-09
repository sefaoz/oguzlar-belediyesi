using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
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
    public async Task<ActionResult<IEnumerable<NewsItem>>> GetAll()
    {
        var news = await _repository.GetAllAsync();
        return Ok(news);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<NewsItem>> GetBySlug(string slug)
    {
        var newsItem = await _repository.GetBySlugAsync(slug);
        
        if (newsItem is not null)
        {
             await _repository.IncrementViewCountAsync(slug);
             await _repository.SaveChangesAsync();
        }

        return newsItem is null ? NotFound() : Ok(newsItem);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("News")]
    public async Task<IActionResult> Create([FromForm] NewsRequest request, [FromForm] IFormFile? file, [FromForm] List<IFormFile>? galleryFiles)
    {
        var image = request.Image ?? string.Empty;

        // Dosya yükleme işlemi
        if (file is not null)
        {
            image = await ImageHelper.SaveImageAsWebPAsync(file, "uploads/news", _env.WebRootPath);
        }

        // Galeri dosyaları yükleme
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

        // Slug oluştur ve benzersizliğini kontrol et
        var slug = await GenerateUniqueSlugAsync(request.Title);

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

        await _repository.AddAsync(newsItem);
        await _repository.SaveChangesAsync();
        return Created($"/api/news/{newsItem.Slug}", newsItem);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("News")]
    public async Task<IActionResult> Update(Guid id, [FromForm] NewsRequest request, [FromForm] IFormFile? file, [FromForm] List<IFormFile>? galleryFiles)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        var image = existing.Image;

        // Dosya yükleme işlemi
        if (file is not null)
        {
            image = await ImageHelper.SaveImageAsWebPAsync(file, "uploads/news", _env.WebRootPath);
        }

        else if (!string.IsNullOrEmpty(request.Image))
        {
            image = request.Image;
        }

        // Galeri dosyaları ve mevcut fotoğraflar
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

        // Title değiştiyse yeni Slug oluştur
        var slug = existing.Slug;
        if (existing.Title != request.Title)
        {
            slug = await GenerateUniqueSlugAsync(request.Title, id);
        }

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.Date = request.Date;
        existing.Slug = slug;
        existing.Image = image;
        existing.Photos = currentPhotos;
        existing.Tags = request.Tags?.ToList();

        await _repository.UpdateAsync(existing);
        await _repository.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("News")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Title'dan benzersiz bir Slug oluşturur.
    /// Eğer aynı Slug veritabanında varsa, sonuna sayı ekleyerek benzersiz yapar.
    /// </summary>
    private async Task<string> GenerateUniqueSlugAsync(string title, Guid? excludeId = null)
    {
        var baseSlug = SlugHelper.GenerateSlug(title);
        var slug = baseSlug;
        var counter = 2;

        while (await _repository.SlugExistsAsync(slug, excludeId))
        {
            slug = SlugHelper.AppendNumber(baseSlug, counter);
            counter++;
        }

        return slug;
    }
}
