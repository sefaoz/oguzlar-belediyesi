using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain.Entities.Configuration;
using OguzlarBelediyesi.WebAPI.Contracts.Requests;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/site-settings")]
public class SiteSettingsController : ControllerBase
{
    private readonly ISiteSettingsRepository _repository;

    public SiteSettingsController(ISiteSettingsRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var settings = await _repository.GetAllAsync(cancellationToken);
        return Ok(settings);
    }

    [HttpGet("group/{groupKey}")]
    public async Task<IActionResult> GetByGroup(string groupKey, CancellationToken cancellationToken)
    {
        var settings = await _repository.GetByGroupAsync(groupKey, cancellationToken);
        return Ok(settings);
    }

    [HttpGet("key/{groupKey}/{key}")]
    public async Task<IActionResult> GetByKey(string groupKey, string key, CancellationToken cancellationToken)
    {
        var setting = await _repository.GetByKeyAsync(groupKey, key, cancellationToken);
        if (setting == null) return NotFound();
        return Ok(setting);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrUpdate([FromBody] SiteSettingRequest request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByKeyAsync(request.GroupKey, request.Key, cancellationToken);
        if (existing != null)
        {
            existing.Value = request.Value;
            existing.Description = request.Description;
            existing.Order = request.Order;
            existing.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(existing, cancellationToken);
            return Ok(existing);
        }
        else
        {
            var setting = new SiteSetting
            {
                Key = request.Key,
                Value = request.Value,
                GroupKey = request.GroupKey,
                Description = request.Description,
                Order = request.Order
            };
            await _repository.AddAsync(setting, cancellationToken);
            return CreatedAtAction(nameof(GetByKey), new { groupKey = setting.GroupKey, key = setting.Key }, setting);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var setting = await _repository.GetByIdAsync(id, cancellationToken);
        if (setting == null) return NotFound();

        await _repository.DeleteAsync(setting, cancellationToken);
        return NoContent();
    }

    [HttpDelete("key/{groupKey}/{key}")]
    [Authorize]
    public async Task<IActionResult> DeleteByKey(string groupKey, string key, CancellationToken cancellationToken)
    {
        var setting = await _repository.GetByKeyAsync(groupKey, key, cancellationToken);
        if (setting == null) return NotFound();

        await _repository.DeleteAsync(setting, cancellationToken);
        return NoContent();
    }
}
