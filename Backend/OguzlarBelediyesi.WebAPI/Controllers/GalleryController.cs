using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Filters;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/gallery")]
public sealed class GalleryController : ControllerBase
{
    private readonly IGalleryRepository _repository;

    public GalleryController(IGalleryRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("folders")]
    [Cache(120)]
    public async Task<ActionResult<IEnumerable<GalleryFolder>>> GetFolders()
    {
        var folders = await _repository.GetFoldersAsync();
        return Ok(folders);
    }

    [HttpGet("folders/{folderId}")]
    [Cache(120)]
    public async Task<ActionResult<GalleryFolder>> GetFolderById(string folderId)
    {
        var folder = await _repository.GetFolderByIdAsync(folderId);
        return folder is null ? NotFound() : Ok(folder);
    }

    [HttpGet("folders/slug/{slug}")]
    [Cache(120)]
    public async Task<ActionResult<GalleryFolder>> GetFolderBySlug(string slug)
    {
        var folder = await _repository.GetFolderBySlugAsync(slug);
        return folder is null ? NotFound() : Ok(folder);
    }

    [HttpGet("folders/{folderId}/images")]
    [Cache(120)]
    public async Task<ActionResult<IEnumerable<GalleryImage>>> GetImagesByFolder(string folderId)
    {
        var images = await _repository.GetImagesByFolderAsync(folderId);
        return Ok(images);
    }
}
