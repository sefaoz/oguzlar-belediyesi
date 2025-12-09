namespace OguzlarBelediyesi.WebAPI.Helpers;

public static class FileHelper
{
    public static async Task<string> SaveFileAsync(IFormFile file, string folderPath, string webRootPath)
    {
        if (file == null || file.Length == 0)
        {
            return string.Empty;
        }

        var extension = Path.GetExtension(file.FileName);
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
