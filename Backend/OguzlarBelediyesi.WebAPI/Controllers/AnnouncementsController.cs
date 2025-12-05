using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Application.Filters;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI;
using OguzlarBelediyesi.WebAPI.Filters;

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
    [Cache(60)]
    public async Task<ActionResult<IEnumerable<Announcement>>> Get([FromQuery] AnnouncementQuery query)
    {
        var filter = new AnnouncementFilter(query.search, query.from, query.to);
        var announcements = await _repository.GetAllAsync(filter);
        return Ok(announcements);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<Announcement>> GetBySlug(string slug)
    {
        var announcement = await _repository.GetBySlugAsync(slug);
        return announcement is null ? NotFound() : Ok(announcement);
    }
}
