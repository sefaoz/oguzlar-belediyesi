using System;
using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Domain;

public sealed class Tender : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime TenderDate { get; set; } = DateTime.UtcNow;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal? EstimatedValue { get; set; }
    public string DocumentsJson { get; set; } = "[]";
    public string Slug { get; set; } = string.Empty;
}
