using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Filters;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/vehicles")]
public sealed class VehiclesController : ControllerBase
{
    private readonly IVehicleRepository _repository;

    public VehiclesController(IVehicleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Cache(120)]
    public async Task<ActionResult<IReadOnlyList<Vehicle>>> GetAll()
    {
        var vehicles = await _repository.GetAllAsync();
        return Ok(vehicles);
    }
}
