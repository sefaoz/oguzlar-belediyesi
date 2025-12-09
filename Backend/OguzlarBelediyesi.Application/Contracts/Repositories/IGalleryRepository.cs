using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IGalleryRepository
{
    Task<IEnumerable<GalleryFolder>> GetFoldersAsync();
    Task<GalleryFolder?> GetFolderByIdAsync(Guid folderId);
    Task<GalleryFolder?> GetFolderBySlugAsync(string slug);
    Task CreateFolderAsync(GalleryFolder folder);
    Task UpdateFolderAsync(GalleryFolder folder);
    Task DeleteFolderAsync(Guid folderId);
    Task<IEnumerable<GalleryImage>> GetImagesByFolderAsync(Guid folderId);
    Task AddImageAsync(GalleryImage image);
    Task DeleteImageAsync(Guid imageId);
}
