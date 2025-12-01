using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OguzlarBelediyesi.Application;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Infrastructure;

public sealed class GalleryRepository : IGalleryRepository
{
    private static readonly IReadOnlyList<GalleryFolder> Folders = new[]
    {
        new GalleryFolder("1", "Oguzlar Ceviz Festivali 2024", "oguzlar-ceviz-festivali-2024", "https://picsum.photos/id/10/800/600", 12, "15.10.2024"),
        new GalleryFolder("2", "Altinkoz Tesisleri Acilisi", "altinkoz-tesisleri-acilisi", "https://picsum.photos/id/20/800/600", 8, "20.09.2024"),
        new GalleryFolder("3", "Doga Yuruyusu Etkinligi", "doga-yuruyusu-etkinligi", "https://picsum.photos/id/28/800/600", 25, "05.09.2024"),
        new GalleryFolder("4", "Obruk Baraji Manzaralari", "obruk-baraji-manzaralari", "https://picsum.photos/id/40/800/600", 15, "01.08.2024"),
        new GalleryFolder("5", "Belediye Calismalari", "belediye-calismalari", "https://picsum.photos/id/50/800/600", 42, "12.07.2024"),
        new GalleryFolder("6", "Koy Manzaralari", "koy-manzaralari", "https://picsum.photos/id/60/800/600", 10, "10.01.2024")
    };

    private static readonly IReadOnlyList<GalleryImage> Images = BuildImages();

    public Task<IEnumerable<GalleryFolder>> GetFoldersAsync()
    {
        return Task.FromResult<IEnumerable<GalleryFolder>>(Folders);
    }

    public Task<GalleryFolder?> GetFolderByIdAsync(string folderId)
    {
        var folder =
            Folders.FirstOrDefault(f => string.Equals(f.Id, folderId, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(folder);
    }

    public Task<GalleryFolder?> GetFolderBySlugAsync(string slug)
    {
        var normalized = slug.Trim();
        var folder = Folders.FirstOrDefault(f => string.Equals(f.Slug, normalized, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(folder);
    }

    public Task<IEnumerable<GalleryImage>> GetImagesByFolderAsync(string folderId)
    {
        var images = Images.Where(img => string.Equals(img.FolderId, folderId, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult<IEnumerable<GalleryImage>>(images);
    }

    private static IReadOnlyList<GalleryImage> BuildImages()
    {
        var built = new List<GalleryImage>();

        foreach (var folder in Folders)
        {
            for (var i = 0; i < folder.ImageCount; i++)
            {
                var imageId = int.Parse(folder.Id) * 10 + i;
                built.Add(new GalleryImage(
                    Id: $"{folder.Id}-{i}",
                    FolderId: folder.Id,
                    Url: $"https://picsum.photos/id/{imageId}/1200/800",
                    ThumbnailUrl: $"https://picsum.photos/id/{imageId}/400/300",
                    Title: $"{folder.Title} - {i + 1}"));
            }
        }

        return built;
    }
}
