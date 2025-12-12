using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Application.Filters;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI;
using OguzlarBelediyesi.WebAPI.Contracts.Requests;
using OguzlarBelediyesi.WebAPI.Filters;
using OguzlarBelediyesi.WebAPI.Helpers;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/events")]
public sealed class EventsController : ControllerBase
{
    private readonly IEventRepository _repository;
    private readonly IWebHostEnvironment _env;

    public EventsController(IEventRepository repository, IWebHostEnvironment env)
    {
        _repository = repository;
        _env = env;
    }

    [HttpGet]
    [Cache(60, "Events")]
    public async Task<ActionResult<IEnumerable<Event>>> Get([FromQuery] EventQuery query, CancellationToken cancellationToken)
    {
        var filter = new EventFilter(query.search, query.upcomingOnly);
        var events = await _repository.GetAllAsync(filter, cancellationToken);
        return Ok(events);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<Event>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var eventItem = await _repository.GetBySlugAsync(slug, cancellationToken);
        return eventItem is null ? NotFound() : Ok(eventItem);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("Events")]
    public async Task<IActionResult> Create([FromForm] EventRequest request, [FromForm] IFormFile? file, CancellationToken cancellationToken)
    {
        var image = request.Image ?? string.Empty;

        if (file is not null)
        {
            image = await ImageHelper.SaveImageAsWebPAsync(file, "uploads/events", _env.WebRootPath);
        }

        var slug = await GenerateUniqueSlugAsync(request.Title, null, cancellationToken);

        var eventItem = new Event
        {
            Id = Guid.NewGuid(),
            Image = image,
            EventDate = request.EventDate,
            EventTime = request.EventTime,
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            Slug = slug
        };

        await _repository.AddAsync(eventItem, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return Created($"/api/events/{eventItem.Slug}", eventItem);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Events")]
    public async Task<IActionResult> Update(Guid id, [FromForm] EventRequest request, [FromForm] IFormFile? file, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

        var image = existing.Image;

        if (file is not null)
        {
            image = await ImageHelper.SaveImageAsWebPAsync(file, "uploads/events", _env.WebRootPath);
        }
        else if (!string.IsNullOrEmpty(request.Image))
        {
            image = request.Image;
        }

        var slug = existing.Slug;
        if (existing.Title != request.Title)
        {
            slug = await GenerateUniqueSlugAsync(request.Title, id, cancellationToken);
        }

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.Location = request.Location;
        existing.EventDate = request.EventDate;
        existing.EventTime = request.EventTime;
        existing.Image = image;
        existing.Slug = slug;

        await _repository.UpdateAsync(existing, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Events")]
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
