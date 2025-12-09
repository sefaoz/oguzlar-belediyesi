using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Filters;
using OguzlarBelediyesi.WebAPI.Helpers;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/kvkk")]
public sealed class KvkkController : ControllerBase
{
    private readonly IKvkkRepository _repository;
    private readonly IWebHostEnvironment _env;

    public KvkkController(IKvkkRepository repository, IWebHostEnvironment env)
    {
        _repository = repository;
        _env = env;
    }

    [HttpGet]
    [Cache(60, "Kvkk")]
    public async Task<ActionResult<IReadOnlyList<KvkkDocument>>> GetAll()
    {
        var documents = await _repository.GetAllAsync();
        return Ok(documents);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("Kvkk")]
    public async Task<IActionResult> Create([FromForm] string title, [FromForm] string type, [FromForm] IFormFile? file)
    {
        var fileUrl = string.Empty;
        if (file != null)
        {
            fileUrl = await FileHelper.SaveFileAsync(file, "uploads/kvkk", _env.WebRootPath);
        }

        var document = new KvkkDocument
        {
            Id = Guid.NewGuid(),
            Title = title,
            Type = type,
            FileUrl = fileUrl
        };

        await _repository.AddAsync(document);
        await _repository.SaveChangesAsync();
        return Ok(document);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Kvkk")]
    public async Task<IActionResult> Update(Guid id, [FromForm] string title, [FromForm] string type, [FromForm] IFormFile? file)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return NotFound();

        if (file != null)
        {
             existing.FileUrl = await FileHelper.SaveFileAsync(file, "uploads/kvkk", _env.WebRootPath);
        }

        existing.Title = title;
        existing.Type = type;

        await _repository.UpdateAsync(existing);
        await _repository.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Kvkk")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
        return NoContent();
    }
}
