namespace OguzlarBelediyesi.WebAPI.Helpers;

public static class FileHelper
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
        ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp",
        ".txt", ".csv", ".zip", ".rar"
    };

    public static async Task<string> SaveFileAsync(IFormFile file, string folderPath, string webRootPath)
    {
        if (file == null || file.Length == 0)
        {
            return string.Empty;
        }

        var extension = Path.GetExtension(file.FileName);
        
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException($"Dosya uzantısı desteklenmiyor: {extension}");
        }

        var fileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(webRootPath, folderPath);

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        var filePath = Path.Combine(fullPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine("/", folderPath, fileName).Replace("\\", "/");
    }
}
