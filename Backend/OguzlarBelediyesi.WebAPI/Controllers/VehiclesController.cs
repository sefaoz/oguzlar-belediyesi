using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Filters;
using OguzlarBelediyesi.WebAPI.Helpers;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/vehicles")]
public sealed class VehiclesController : ControllerBase
{
    private readonly IVehicleRepository _repository;
    private readonly IWebHostEnvironment _env;

    public VehiclesController(IVehicleRepository repository, IWebHostEnvironment env)
    {
        _repository = repository;
        _env = env;
    }

    [HttpGet]
    [Cache(120, "Vehicles")]
    public async Task<ActionResult<IReadOnlyList<Vehicle>>> GetAll()
    {
        var vehicles = await _repository.GetAllAsync();
        return Ok(vehicles);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("Vehicles")]
    public async Task<IActionResult> Create([FromForm] VehicleRequest request, [FromForm] IFormFile? file)
    {
        var image = request.ImageUrl ?? string.Empty;

        if (file is not null)
        {
            image = await ImageHelper.SaveImageAsWebPAsync(file, "uploads/vehicles", _env.WebRootPath);
        }

        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Type = request.Type,
            Plate = request.Plate,
            Description = request.Description,
            ImageUrl = image
        };

        await _repository.AddAsync(vehicle);
        await _repository.SaveChangesAsync();
        return Created($"/api/vehicles/{vehicle.Id}", vehicle);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Vehicles")]
    public async Task<IActionResult> Update(Guid id, [FromForm] VehicleRequest request, [FromForm] IFormFile? file)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        var image = existing.ImageUrl;

        if (file is not null)
        {
            image = await ImageHelper.SaveImageAsWebPAsync(file, "uploads/vehicles", _env.WebRootPath);
        }
        else if (!string.IsNullOrEmpty(request.ImageUrl))
        {
            image = request.ImageUrl;
        }

        existing.Name = request.Name;
        existing.Type = request.Type;
        existing.Plate = request.Plate;
        existing.Description = request.Description;
        existing.ImageUrl = image;

        await _repository.UpdateAsync(existing);
        await _repository.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Vehicles")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
        return NoContent();
    }
}
