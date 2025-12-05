using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Application.Filters;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI;
using OguzlarBelediyesi.WebAPI.Filters;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/tenders")]
public sealed class TendersController : ControllerBase
{
    private readonly ITenderRepository _repository;

    public TendersController(ITenderRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Cache(60)]
    public async Task<ActionResult<IEnumerable<Tender>>> Get([FromQuery] TenderQuery query)
    {
        var filter = new TenderFilter(query.search, query.status);
        var tenders = await _repository.GetAllAsync(filter);
        return Ok(tenders);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<Tender>> GetBySlug(string slug)
    {
        var tender = await _repository.GetBySlugAsync(slug);
        return tender is null ? NotFound() : Ok(tender);
    }
}
