using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Domain.Types;
using Tauchbolde.Domain.ValueObjects;

namespace Tauchbolde.Application.Services.PhotoStores
{
    /// <summary>
    /// Service providing access to the photo storage backend.
    /// </summary>
    public interface IPhotoService
    {
        // TODO Change to more high-level API
        
        /// <summary>
        /// Stores a photo in the photo storage and returns its identifier.
        /// </summary>
        /// <returns>The unique photo identifier of the added photo.</returns>
        Task<PhotoAndThumbnailIdentifiers> AddPhotoAsync(
            PhotoCategory category,
            [NotNull] Stream photoData,
            [NotNull] string contentType,
            string filename);

        Task<PhotoAndThumbnailIdentifiers> UpdatePhotoAsync(
            [CanBeNull] PhotoAndThumbnailIdentifiers existingPhoto,
            PhotoCategory category,
            [CanBeNull] Stream photoData,
            [CanBeNull] string filename,
            [CanBeNull] string contentType);

        /// <summary>
        /// Removes existing photos from the storage.
        /// </summary>
        /// <param name="photoIdentifiers">The unique identifier of the photo to remove.</param>
        Task RemovePhotosAsync([NotNull] params PhotoIdentifier[] photoIdentifiers);

        /// <summary>
        /// Retrieves a photo from the photo storage including meta- and binary-data.
        /// </summary>
        /// <param name="photoIdentifier">The unique identifier of the photo to retrieve</param>
        /// <returns>Photo from the photo storage including meta- and binary-data.</returns>
        Task<Photo> GetPhotoDataAsync([NotNull] PhotoIdentifier photoIdentifier);
    }
}