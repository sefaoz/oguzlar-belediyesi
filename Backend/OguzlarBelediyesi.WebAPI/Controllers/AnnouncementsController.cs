using System;
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
[Route("api/announcements")]
public sealed class AnnouncementsController : ControllerBase
{
    private readonly IAnnouncementRepository _repository;

    public AnnouncementsController(IAnnouncementRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Cache(60, "Announcements")]
    public async Task<ActionResult<IEnumerable<Announcement>>> Get([FromQuery] AnnouncementQuery query, CancellationToken cancellationToken)
    {
        var filter = new AnnouncementFilter(query.search, query.from, query.to);
        var announcements = await _repository.GetAllAsync(filter, cancellationToken);
        return Ok(announcements);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<Announcement>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var announcement = await _repository.GetBySlugAsync(slug, cancellationToken);
        return announcement is null ? NotFound() : Ok(announcement);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("Announcements")]
    public async Task<IActionResult> Create([FromForm] AnnouncementRequest request, CancellationToken cancellationToken)
    {
        var slug = await GenerateUniqueSlugAsync(request.Title, null, cancellationToken);

        var announcement = new Announcement
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Summary = request.Summary,
            Content = request.Content,
            Category = request.Category,
            Date = request.Date,
            Slug = slug
        };

        await _repository.AddAsync(announcement, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return Created($"/api/announcements/{announcement.Slug}", announcement);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Announcements")]
    public async Task<IActionResult> Update(Guid id, [FromForm] AnnouncementRequest request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

        var slug = existing.Slug;
        if (existing.Title != request.Title)
        {
            slug = await GenerateUniqueSlugAsync(request.Title, id, cancellationToken);
        }

        existing.Title = request.Title;
        existing.Summary = request.Summary;
        existing.Content = request.Content;
        existing.Category = request.Category;
        existing.Date = request.Date;
        existing.Slug = slug;
        existing.UpdateDate = DateTime.UtcNow;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Announcements")]
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
