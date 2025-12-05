using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/kvkk")]
public sealed class KvkkController : ControllerBase
{
    private readonly IKvkkRepository _repository;

    public KvkkController(IKvkkRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<KvkkDocument>>> GetAll()
    {
        var documents = await _repository.GetAllAsync();
        return Ok(documents);
    }
}
