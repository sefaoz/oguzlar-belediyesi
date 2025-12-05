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
    [Cache(60)]
    public async Task<ActionResult<IEnumerable<PageContent>>> GetAll()
    {
        var pages = await _repository.GetAllAsync();
        return Ok(pages);
    }

    [HttpGet("{key}")]
    [Cache(120)]
    public async Task<ActionResult<PageContent>> GetByKey(string key)
    {
        var pageContent = await _repository.GetByKeyAsync(key);
        return pageContent is null ? NotFound() : Ok(pageContent);
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var form = await Request.ReadFormAsync();
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

        var pageContent = new PageContent(Guid.NewGuid(), formData.Key, formData.Title, formData.Subtitle, formData.Paragraphs, imageUrl, formData.MapEmbedUrl, formData.ContactDetails);
        await _repository.AddAsync(pageContent);
        return CreatedAtAction(nameof(GetByKey), new { key = pageContent.Key }, pageContent);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        var form = await Request.ReadFormAsync();
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
        var updated = existing with
        {
            Key = formData.Key,
            Title = formData.Title,
            Subtitle = formData.Subtitle,
            Paragraphs = formData.Paragraphs,
            ImageUrl = updatedImageUrl,
            MapEmbedUrl = formData.MapEmbedUrl,
            ContactDetails = formData.ContactDetails
        };

        await _repository.UpdateAsync(updated);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }

    private string GetBaseUri()
    {
        return $"{Request.Scheme}://{Request.Host}";
    }
}
