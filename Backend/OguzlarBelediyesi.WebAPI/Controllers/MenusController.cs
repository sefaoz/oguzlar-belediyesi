using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Contracts.Requests;
using OguzlarBelediyesi.WebAPI.Filters;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/menus")]
public sealed class MenusController : ControllerBase
{
    private readonly IMenuRepository _repository;

    public MenusController(IMenuRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Cache(60, "Menus")]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetAll(CancellationToken cancellationToken)
    {
        var menus = await _repository.GetAllAsync(cancellationToken);
        return Ok(menus);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MenuItem>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var menu = await _repository.GetByIdAsync(id, cancellationToken);
        return menu is null ? NotFound() : Ok(menu);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("Menus")]
    public async Task<IActionResult> Create(MenuRequest request, CancellationToken cancellationToken)
    {
        var menuItem = new MenuItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Url = request.Url,
            ParentId = request.ParentId,
            Order = request.Order,
            IsVisible = request.IsVisible,
            Target = request.Target
        };

        await _repository.AddAsync(menuItem, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = menuItem.Id }, menuItem);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Menus")]
    public async Task<IActionResult> Update(Guid id, MenuRequest request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Title = request.Title;
        existing.Url = request.Url;
        existing.ParentId = request.ParentId;
        existing.Order = request.Order;
        existing.IsVisible = request.IsVisible;
        existing.Target = request.Target;

        await _repository.UpdateAsync(existing, cancellationToken);
        return NoContent();
    }

    [HttpPut("order")]
    [Authorize]
    [CacheInvalidate("Menus")]
    public async Task<IActionResult> UpdateOrder([FromBody] IEnumerable<MenuOrderRequest>? updates, CancellationToken cancellationToken)
    {
        if (updates is null)
        {
            return BadRequest();
        }

        var currentMenus = (await _repository.GetAllAsync(cancellationToken)).ToDictionary(m => m.Id);
        var orderedUpdates = new List<MenuItem>();

        foreach (var u in updates)
        {
            if (currentMenus.TryGetValue(u.Id, out var existing))
            {
                existing.Order = u.Order;
                existing.ParentId = u.ParentId;
                orderedUpdates.Add(existing);
            }
        }

        if (orderedUpdates.Count == 0)
        {
            return BadRequest();
        }

        await _repository.UpdateOrderAsync(orderedUpdates, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Menus")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
