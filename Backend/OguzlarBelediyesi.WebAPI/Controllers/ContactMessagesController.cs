using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain.Entities.Messages;
using OguzlarBelediyesi.WebAPI.Contracts.Requests;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/contact-messages")]
public sealed class ContactMessagesController : ControllerBase
{
    private readonly IContactMessageRepository _repository;

    public ContactMessagesController(IContactMessageRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var messages = await _repository.GetAllAsync(cancellationToken);
        return Ok(messages);
    }

    [HttpGet("type/{messageType}")]
    [Authorize]
    public async Task<IActionResult> GetByType(string messageType, CancellationToken cancellationToken)
    {
        var messages = await _repository.GetByTypeAsync(messageType, cancellationToken);
        return Ok(messages);
    }

    [HttpGet("unread-count")]
    [Authorize]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
    {
        var count = await _repository.GetUnreadCountAsync(cancellationToken);
        return Ok(new { count });
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var message = await _repository.GetByIdAsync(id, cancellationToken);
        if (message is null) return NotFound();
        return Ok(message);
    }

    [HttpPost("contact")]
    [EnableRateLimiting("fixed")]
    public async Task<IActionResult> CreateContactMessage([FromBody] ContactMessageRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var message = new ContactMessage
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Message = request.Message,
            MessageType = "Contact",
            KvkkAccepted = request.KvkkAccepted,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(message, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Ok(new { success = true, message = "Mesajınız başarıyla iletildi." });
    }

    [HttpPost("mayor")]
    [EnableRateLimiting("fixed")]
    public async Task<IActionResult> CreateMayorMessage([FromBody] ContactMessageRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var message = new ContactMessage
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Message = request.Message,
            MessageType = "MayorMessage",
            KvkkAccepted = request.KvkkAccepted,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(message, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Ok(new { success = true, message = "Mesajınız Başkana iletilmek üzere kaydedildi." });
    }

    [HttpPut("{id:guid}/mark-read")]
    [Authorize]
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
    {
        var message = await _repository.GetByIdAsync(id, cancellationToken);
        if (message is null) return NotFound();

        message.IsRead = true;
        await _repository.UpdateAsync(message, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
