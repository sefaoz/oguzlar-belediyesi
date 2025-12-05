using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Application.Filters;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI;
using OguzlarBelediyesi.WebAPI.Filters;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/events")]
public sealed class EventsController : ControllerBase
{
    private readonly IEventRepository _repository;

    public EventsController(IEventRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Cache(60)]
    public async Task<ActionResult<IEnumerable<Event>>> Get([FromQuery] EventQuery query)
    {
        var filter = new EventFilter(query.search, query.upcomingOnly);
        var events = await _repository.GetAllAsync(filter);
        return Ok(events);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<Event>> GetBySlug(string slug)
    {
        var eventItem = await _repository.GetBySlugAsync(slug);
        return eventItem is null ? NotFound() : Ok(eventItem);
    }
}
