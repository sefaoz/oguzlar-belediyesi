using System;

namespace OguzlarBelediyesi.Domain.Entities.Configuration
{
    public class SiteSetting
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string GroupKey { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
