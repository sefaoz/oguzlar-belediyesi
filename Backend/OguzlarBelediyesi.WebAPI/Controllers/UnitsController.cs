using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Filters;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/units")]
public sealed class UnitsController : ControllerBase
{
    private readonly IMunicipalUnitRepository _repository;

    public UnitsController(IMunicipalUnitRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Cache(120)]
    public async Task<ActionResult<IReadOnlyList<MunicipalUnit>>> GetAll()
    {
        var units = await _repository.GetAllAsync();
        return Ok(units);
    }
}
