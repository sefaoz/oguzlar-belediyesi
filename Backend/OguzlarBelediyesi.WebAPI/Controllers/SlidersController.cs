using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Filters;
using OguzlarBelediyesi.WebAPI.Helpers;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/sliders")]
public sealed class SlidersController : ControllerBase
{
    private readonly ISliderRepository _repository;
    private readonly IWebHostEnvironment _env;

    public SlidersController(ISliderRepository repository, IWebHostEnvironment env)
    {
        _repository = repository;
        _env = env;
    }

    [HttpGet]
    [Cache(60, "Sliders")]
    public async Task<ActionResult<IEnumerable<Slider>>> GetAll()
    {
        var sliders = await _repository.GetAllAsync();
        return Ok(sliders);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("Sliders")]
    public async Task<IActionResult> Create([FromForm] SliderFormRequest request)
    {
        string imageUrl = string.Empty;

        // Görsel dosyası varsa yükle
        if (request.File != null && request.File.Length > 0)
        {
            imageUrl = await ImageHelper.SaveImageAsWebPAsync(request.File, "uploads/sliders", _env.WebRootPath);
        }
        else if (!string.IsNullOrWhiteSpace(request.ImageUrl))
        {
            // Mevcut imageUrl varsa kullan
            imageUrl = request.ImageUrl;
        }
        else
        {
            return BadRequest("Görsel zorunludur.");
        }

        var slider = new Slider
        {
            Id = Guid.NewGuid(),
            Title = request.Title ?? string.Empty,
            Description = request.Description ?? string.Empty,
            ImageUrl = imageUrl,
            Link = request.Link,
            Order = request.Order,
            IsActive = request.IsActive
        };

        await _repository.AddAsync(slider);
        await _repository.SaveChangesAsync();
        return Created($"/api/sliders/{slider.Id}", slider);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Sliders")]
    public async Task<IActionResult> Update(Guid id, [FromForm] SliderFormRequest request)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        // Yeni görsel dosyası varsa yükle
        if (request.File != null && request.File.Length > 0)
        {
            existing.ImageUrl = await ImageHelper.SaveImageAsWebPAsync(request.File, "uploads/sliders", _env.WebRootPath);
        }
        else if (!string.IsNullOrWhiteSpace(request.ImageUrl))
        {
            existing.ImageUrl = request.ImageUrl;
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            existing.Title = request.Title;
        }
        
        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            existing.Description = request.Description;
        }
        
        existing.Link = request.Link;
        existing.Order = request.Order;
        existing.IsActive = request.IsActive;

        await _repository.UpdateAsync(existing);
        await _repository.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Sliders")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
        return NoContent();
    }
}

// FormData ile görsel yüklemeyi destekleyen request sınıfı
public class SliderFormRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Link { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public IFormFile? File { get; set; }
}
