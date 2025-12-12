using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Domain.Entities.Messages;

public class ContactMessage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Message { get; set; } = string.Empty;
    
    public string MessageType { get; set; } = "Contact";
    
    public bool KvkkAccepted { get; set; }
    
    public bool IsRead { get; set; }
    
    public string? IpAddress { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
