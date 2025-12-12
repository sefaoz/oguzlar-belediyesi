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
    public async Task<ActionResult<IEnumerable<Slider>>> GetAll(CancellationToken cancellationToken)
    {
        var sliders = await _repository.GetAllAsync(cancellationToken);
        return Ok(sliders);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("Sliders")]
    public async Task<IActionResult> Create([FromForm] SliderFormRequest request, CancellationToken cancellationToken)
    {
        string imageUrl = string.Empty;

        if (request.File != null && request.File.Length > 0)
        {
            imageUrl = await ImageHelper.SaveImageAsWebPAsync(request.File, "uploads/sliders", _env.WebRootPath);
        }
        else if (!string.IsNullOrWhiteSpace(request.ImageUrl))
        {
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

        await _repository.AddAsync(slider, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return Created($"/api/sliders/{slider.Id}", slider);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Sliders")]
    public async Task<IActionResult> Update(Guid id, [FromForm] SliderFormRequest request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

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

        await _repository.UpdateAsync(existing, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Sliders")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
