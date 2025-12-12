using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Filters;
using OguzlarBelediyesi.WebAPI.Helpers;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/pages")]
public sealed class PageContentsController : ControllerBase
{
    private readonly IPageContentRepository _repository;
    private readonly IWebHostEnvironment _environment;

    public PageContentsController(IPageContentRepository repository, IWebHostEnvironment environment)
    {
        _repository = repository;
        _environment = environment;
    }

    [HttpGet]
    [Cache(60, "PageContents")]
    public async Task<ActionResult<IEnumerable<PageContent>>> GetAll(CancellationToken cancellationToken)
    {
        var pages = await _repository.GetAllAsync(cancellationToken);
        return Ok(pages);
    }

    [HttpGet("{key}")]
    [Cache(120, "PageContents")]
    public async Task<ActionResult<PageContent>> GetByKey(string key, CancellationToken cancellationToken)
    {
        var pageContent = await _repository.GetByKeyAsync(key, cancellationToken);
        return pageContent is null ? NotFound() : Ok(pageContent);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("PageContents")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var form = await Request.ReadFormAsync(cancellationToken);
        var formData = PageContentUploadHelper.ParsePageContentForm(form);
        if (string.IsNullOrWhiteSpace(formData.Key) || string.IsNullOrWhiteSpace(formData.Title))
        {
            return BadRequest(new { message = "Anahtar ve başlık alanları gereklidir." });
        }

        var baseUri = GetBaseUri();
        var webRootPath = PageContentUploadHelper.EnsureWebRootPath(_environment);
        var file = form.Files.GetFile("file");
        var savedImageUrl = await PageContentUploadHelper.SavePageContentImageAsync(file, formData.Key, baseUri, webRootPath);
        var imageUrl = savedImageUrl ?? formData.ImageUrl;

        var pageContent = new PageContent
        {
            Id = Guid.NewGuid(),
            Key = formData.Key,
            Title = formData.Title,
            Subtitle = formData.Subtitle,
            Paragraphs = formData.Paragraphs?.ToList() ?? new List<string>(),
            ImageUrl = imageUrl,
            MapEmbedUrl = formData.MapEmbedUrl,
            ContactDetails = formData.ContactDetails?.ToList()
        };

        await _repository.AddAsync(pageContent, cancellationToken);
        return CreatedAtAction(nameof(GetByKey), new { key = pageContent.Key }, pageContent);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("PageContents")]
    public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

        var form = await Request.ReadFormAsync(cancellationToken);
        var formData = PageContentUploadHelper.ParsePageContentForm(form);
        if (string.IsNullOrWhiteSpace(formData.Key) || string.IsNullOrWhiteSpace(formData.Title))
        {
            return BadRequest(new { message = "Anahtar ve başlık alanları gereklidir." });
        }

        var baseUri = GetBaseUri();
        var webRootPath = PageContentUploadHelper.EnsureWebRootPath(_environment);
        var file = form.Files.GetFile("file");
        var savedImageUrl = await PageContentUploadHelper.SavePageContentImageAsync(file, formData.Key, baseUri, webRootPath);
        if (savedImageUrl is not null)
        {
            PageContentUploadHelper.DeleteStoredImageIfLocal(existing.ImageUrl, baseUri, webRootPath);
        }

        var updatedImageUrl = savedImageUrl ?? existing.ImageUrl ?? formData.ImageUrl;
        
        existing.Key = formData.Key;
        existing.Title = formData.Title;
        existing.Subtitle = formData.Subtitle;
        existing.Paragraphs = formData.Paragraphs?.ToList() ?? new List<string>();
        existing.ImageUrl = updatedImageUrl;
        existing.MapEmbedUrl = formData.MapEmbedUrl;
        existing.ContactDetails = formData.ContactDetails?.ToList();

        await _repository.UpdateAsync(existing, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("PageContents")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    private string GetBaseUri()
    {
        return $"{Request.Scheme}://{Request.Host}";
    }
}
