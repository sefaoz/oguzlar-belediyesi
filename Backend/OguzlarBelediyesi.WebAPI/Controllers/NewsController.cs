using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Filters;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/news")]
public sealed class NewsController : ControllerBase
{
    private readonly INewsRepository _repository;

    public NewsController(INewsRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Cache(60)]
    public async Task<ActionResult<IEnumerable<NewsItem>>> GetAll()
    {
        var news = await _repository.GetAllAsync();
        return Ok(news);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<NewsItem>> GetBySlug(string slug)
    {
        var newsItem = await _repository.GetBySlugAsync(slug);
        return newsItem is null ? NotFound() : Ok(newsItem);
    }
}
