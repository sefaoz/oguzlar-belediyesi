using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IGalleryRepository
{
    Task<IEnumerable<GalleryFolder>> GetFoldersAsync(CancellationToken cancellationToken = default);
    Task<GalleryFolder?> GetFolderByIdAsync(Guid folderId, CancellationToken cancellationToken = default);
    Task<GalleryFolder?> GetFolderBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task CreateFolderAsync(GalleryFolder folder, CancellationToken cancellationToken = default);
    Task UpdateFolderAsync(GalleryFolder folder, CancellationToken cancellationToken = default);
    Task DeleteFolderAsync(Guid folderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GalleryImage>> GetImagesByFolderAsync(Guid folderId, CancellationToken cancellationToken = default);
    Task AddImageAsync(GalleryImage image, CancellationToken cancellationToken = default);
    Task DeleteImageAsync(Guid imageId, CancellationToken cancellationToken = default);
}
