namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class SliderEntity
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string? Link { get; set; }

    public int Order { get; set; }

    public bool IsActive { get; set; }
}
