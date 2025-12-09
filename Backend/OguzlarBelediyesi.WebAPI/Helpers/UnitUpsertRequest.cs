
namespace OguzlarBelediyesi.WebAPI.Helpers;

public class UnitUpsertRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    
    // JSON string for existing/updated metadata of staff
    public string? StaffJson { get; set; } 
}
