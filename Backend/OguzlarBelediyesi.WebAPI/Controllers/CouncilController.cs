using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Filters;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/meclis")]
public sealed class CouncilController : ControllerBase
{
    private readonly ICouncilRepository _repository;
    private readonly IWebHostEnvironment _env;

    public CouncilController(ICouncilRepository repository, IWebHostEnvironment env)
    {
        _repository = repository;
        _env = env;
    }

    [HttpGet]
    [Cache(60, "Council")]
    public async Task<ActionResult<IReadOnlyList<CouncilDocument>>> GetAll(CancellationToken cancellationToken)
    {
        var documents = await _repository.GetAllAsync(cancellationToken);
        return Ok(documents);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CouncilDocument>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var document = await _repository.GetByIdAsync(id, cancellationToken);
        return document is null ? NotFound() : Ok(document);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("Council")]
    public async Task<IActionResult> Create([FromForm] CouncilDocumentRequest request, [FromForm] IFormFile? file, CancellationToken cancellationToken)
    {
        var fileUrl = string.Empty;
        if (file is not null)
        {
            fileUrl = await SaveFileAsync(file, "uploads/council", cancellationToken);
        }

        var document = new CouncilDocument
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Type = request.Type,
            Date = request.Date,
            Description = request.Description,
            FileUrl = fileUrl
        };

        await _repository.AddAsync(document, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Created($"/api/meclis/{document.Id}", document);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Council")]
    public async Task<IActionResult> Update(Guid id, [FromForm] CouncilDocumentRequest request, [FromForm] IFormFile? file, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

        var fileUrl = existing.FileUrl;
        if (file is not null)
        {
            fileUrl = await SaveFileAsync(file, "uploads/council", cancellationToken);
        }

        existing.Title = request.Title;
        existing.Type = request.Type;
        existing.Date = request.Date;
        existing.Description = request.Description;
        existing.FileUrl = fileUrl;

        await _repository.UpdateAsync(existing, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Council")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private async Task<string> SaveFileAsync(IFormFile file, string folderPath, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0) return string.Empty;
        
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(_env.WebRootPath, folderPath);
        
        if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
        
        var filePath = Path.Combine(fullPath, fileName);
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }
        
        return Path.Combine("/", folderPath, fileName).Replace("\\", "/");
    }
}

public class CouncilDocumentRequest
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}
