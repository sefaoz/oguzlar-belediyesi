using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/meclis")]
public sealed class CouncilController : ControllerBase
{
    private readonly ICouncilRepository _repository;

    public CouncilController(ICouncilRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CouncilDocument>>> GetAll()
    {
        var documents = await _repository.GetAllAsync();
        return Ok(documents);
    }
}
