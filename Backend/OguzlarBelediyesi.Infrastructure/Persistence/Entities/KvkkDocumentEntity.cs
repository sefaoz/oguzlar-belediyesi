namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class KvkkDocumentEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string FileUrl { get; set; } = string.Empty;
}
