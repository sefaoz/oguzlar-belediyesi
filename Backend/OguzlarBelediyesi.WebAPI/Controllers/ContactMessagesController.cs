using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain.Entities.Messages;

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

    /// <summary>
    /// Tüm mesajları getirir (Admin için).
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var messages = await _repository.GetAllAsync();
        return Ok(messages);
    }

    /// <summary>
    /// Belirli tipteki mesajları getirir.
    /// </summary>
    [HttpGet("type/{messageType}")]
    [Authorize]
    public async Task<IActionResult> GetByType(string messageType)
    {
        var messages = await _repository.GetByTypeAsync(messageType);
        return Ok(messages);
    }

    /// <summary>
    /// Okunmamış mesaj sayısını getirir.
    /// </summary>
    [HttpGet("unread-count")]
    [Authorize]
    public async Task<IActionResult> GetUnreadCount()
    {
        var count = await _repository.GetUnreadCountAsync();
        return Ok(new { count });
    }

    /// <summary>
    /// Belirli bir mesajı getirir.
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var message = await _repository.GetByIdAsync(id);
        if (message is null) return NotFound();
        return Ok(message);
    }

    /// <summary>
    /// Yeni iletişim mesajı gönderir (Public - Rate Limited).
    /// </summary>
    [HttpPost("contact")]
    [EnableRateLimiting("fixed")]
    public async Task<IActionResult> CreateContactMessage([FromBody] ContactMessageRequest request)
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

        await _repository.AddAsync(message);
        await _repository.SaveChangesAsync();

        return Ok(new { success = true, message = "Mesajınız başarıyla iletildi." });
    }

    /// <summary>
    /// Başkana mesaj gönderir (Public - Rate Limited).
    /// </summary>
    [HttpPost("mayor")]
    [EnableRateLimiting("fixed")]
    public async Task<IActionResult> CreateMayorMessage([FromBody] ContactMessageRequest request)
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

        await _repository.AddAsync(message);
        await _repository.SaveChangesAsync();

        return Ok(new { success = true, message = "Mesajınız Başkana iletilmek üzere kaydedildi." });
    }

    /// <summary>
    /// Mesajı okundu olarak işaretler.
    /// </summary>
    [HttpPut("{id:guid}/mark-read")]
    [Authorize]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var message = await _repository.GetByIdAsync(id);
        if (message is null) return NotFound();

        message.IsRead = true;
        await _repository.UpdateAsync(message);
        await _repository.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Mesajı siler (Soft delete yerine hard delete).
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
        return NoContent();
    }
}

/// <summary>
/// İletişim mesajı gönderme isteği modeli.
/// </summary>
public record ContactMessageRequest(
    string Name,
    string Email,
    string Phone,
    string Message,
    bool KvkkAccepted
);
