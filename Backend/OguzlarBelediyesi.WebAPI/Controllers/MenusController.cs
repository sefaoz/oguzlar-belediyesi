using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI;
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
    [Cache(60)]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetAll()
    {
        var menus = await _repository.GetAllAsync();
        return Ok(menus);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MenuItem>> GetById(Guid id)
    {
        var menu = await _repository.GetByIdAsync(id);
        return menu is null ? NotFound() : Ok(menu);
    }

    [HttpPost]
    public async Task<IActionResult> Create(MenuRequest request)
    {
        var menuItem = new MenuItem(Guid.NewGuid(), request.Title, request.Url, request.ParentId, request.Order, request.IsVisible, request.Target);
        await _repository.AddAsync(menuItem);
        return CreatedAtAction(nameof(GetById), new { id = menuItem.Id }, menuItem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, MenuRequest request)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        var updated = existing with
        {
            Title = request.Title,
            Url = request.Url,
            ParentId = request.ParentId,
            Order = request.Order,
            IsVisible = request.IsVisible,
            Target = request.Target
        };

        await _repository.UpdateAsync(updated);
        return NoContent();
    }

    [HttpPut("order")]
    public async Task<IActionResult> UpdateOrder([FromBody] IEnumerable<MenuOrderRequest>? updates)
    {
        if (updates is null)
        {
            return BadRequest();
        }

        var currentMenus = (await _repository.GetAllAsync()).ToDictionary(m => m.Id);
        var orderedUpdates = updates
            .Where(u => currentMenus.ContainsKey(u.Id))
            .Select(u => currentMenus[u.Id] with { Order = u.Order, ParentId = u.ParentId })
            .ToList();

        if (orderedUpdates.Count == 0)
        {
            return BadRequest();
        }

        await _repository.UpdateOrderAsync(orderedUpdates);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
