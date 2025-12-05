using System;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI;
using OguzlarBelediyesi.WebAPI.Filters;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/sliders")]
public sealed class SlidersController : ControllerBase
{
    private readonly ISliderRepository _repository;

    public SlidersController(ISliderRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Cache(60)]
    public async Task<ActionResult<IEnumerable<Slider>>> GetAll()
    {
        var sliders = await _repository.GetAllAsync();
        return Ok(sliders);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SliderRequest request)
    {
        var slider = new Slider(Guid.NewGuid().ToString(), request.Title, request.Description, request.ImageUrl, request.Link, request.Order, request.IsActive);
        await _repository.AddAsync(slider);
        await _repository.SaveChangesAsync();
        return Created($"/api/sliders/{slider.Id}", slider);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, SliderRequest request)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        var updated = existing with
        {
            Title = request.Title,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Link = request.Link,
            Order = request.Order,
            IsActive = request.IsActive
        };

        await _repository.UpdateAsync(updated);
        await _repository.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
        return NoContent();
    }
}
