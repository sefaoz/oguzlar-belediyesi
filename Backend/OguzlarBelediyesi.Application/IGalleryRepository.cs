using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application;

public interface IGalleryRepository
{
    Task<IEnumerable<GalleryFolder>> GetFoldersAsync();
    Task<GalleryFolder?> GetFolderByIdAsync(string folderId);
    Task<GalleryFolder?> GetFolderBySlugAsync(string slug);
    Task<IEnumerable<GalleryImage>> GetImagesByFolderAsync(string folderId);
}
