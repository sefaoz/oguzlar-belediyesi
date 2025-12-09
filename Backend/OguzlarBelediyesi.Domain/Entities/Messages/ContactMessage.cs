using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Domain.Entities.Messages;

/// <summary>
/// İletişim ve Başkana Mesaj formlarından gelen mesajları temsil eder.
/// </summary>
public class ContactMessage : BaseEntity
{
    /// <summary>
    /// Mesajı gönderen kişinin adı soyadı.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Mesajı gönderen kişinin e-posta adresi.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Mesajı gönderen kişinin telefon numarası.
    /// </summary>
    public string Phone { get; set; } = string.Empty;
    
    /// <summary>
    /// Mesaj içeriği.
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Mesajın tipi: "Contact" (İletişim), "MayorMessage" (Başkana Mesaj)
    /// </summary>
    public string MessageType { get; set; } = "Contact";
    
    /// <summary>
    /// KVKK onayı verildi mi?
    /// </summary>
    public bool KvkkAccepted { get; set; }
    
    /// <summary>
    /// Mesajın okunma durumu.
    /// </summary>
    public bool IsRead { get; set; }
    
    /// <summary>
    /// Gönderen IP adresi (güvenlik ve spam önleme için).
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// Mesajın gönderildiği tarih.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
