using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace OguzlarBelediyesi.WebAPI.Helpers;

public static class ImageHelper
{
    public static async Task<string> SaveImageAsWebPAsync(IFormFile file, string folderPath, string webRootPath)
    {
        if (file == null || file.Length == 0)
        {
            return string.Empty;
        }

        var fileName = $"{Guid.NewGuid()}.webp";
        var fullPath = Path.Combine(webRootPath, folderPath);

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        var filePath = Path.Combine(fullPath, fileName);

        using (var image = await Image.LoadAsync(file.OpenReadStream()))
        {
            await image.SaveAsWebpAsync(filePath, new WebpEncoder { Quality = 80 });
        }

        // Return URL path (convert backslashes to forward slashes for URL compatibility)
        return Path.Combine("/", folderPath, fileName).Replace("\\", "/");
    }
}
