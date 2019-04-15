using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tauchbolde.Common.DomainServices.PhotoStorage
{
    /// <summary>
    /// Default implementation of <see cref="IPhotoService"/>.
    /// </summary>
    internal class PhotoService: IPhotoService
    {
        [NotNull] private readonly IPhotoStore photoStore;
        [NotNull] private readonly IImageResizer imageResizer;

        public PhotoService(
            [NotNull] IPhotoStore photoStore,
            [NotNull] IImageResizer imageResizer)
        {
            this.photoStore = photoStore ?? throw new ArgumentNullException(nameof(photoStore));
            this.imageResizer = imageResizer ?? throw new ArgumentNullException(nameof(imageResizer));
        }
        
        public async Task<PhotoAndThumbnailIdentification> AddPhotoAsync(
            PhotoCategory category,
            Stream photoData,
            string filename,
            string contentType,
            ThumbnailType thumbnailType)
        {
            if (photoData == null) throw new ArgumentNullException(nameof(photoData));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            
            return await AddPhotoWithThumbnail(photoData, filename, contentType, thumbnailType);
        }

        public async Task<PhotoAndThumbnailIdentification> UpdatePhotoAsync(
            PhotoAndThumbnailIdentification existingPhoto,
            PhotoCategory category,
            Stream photoData,
            string filename,
            string contentType,
            ThumbnailType thumbnailType)
        {
            if (photoData == null) throw new ArgumentNullException(nameof(photoData));

            if (existingPhoto != null)
            {
                await RemovePhotosAsync(new[]
                {
                    existingPhoto.OriginalPhotoIdentifier,
                    existingPhoto.ThumbnailPhotoIdentifier,
                });
            }

            return await AddPhotoWithThumbnail(photoData, filename, contentType, thumbnailType);
        }

        public async Task RemovePhotosAsync(params PhotoIdentifier[] photoIdentifiers)
            => await Task.WhenAll(
                photoIdentifiers
                    .Select(i => photoStore.RemovePhotoAsync(i))
                    .ToArray());

        public async Task<Photo> GetPhotoDataAsync(PhotoIdentifier photoIdentifier)
        {
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            return await photoStore.GetPhotoAsync(photoIdentifier);
        }
        
        private async Task<PhotoAndThumbnailIdentification> AddPhotoWithThumbnail(
            [NotNull] Stream photoData,
            string filename,
            string contentType,
            ThumbnailType thumbnailType)
        {
            if (photoData == null) throw new ArgumentNullException(nameof(photoData));

            return new PhotoAndThumbnailIdentification(
                await AddOriginalPhoto(photoData, filename, contentType),
                await AddThumbnail(photoData, filename, contentType, thumbnailType));
        }

        private async Task<PhotoIdentifier> AddOriginalPhoto(Stream photoData, string filename, string contentType)
        {
            var photo = new Photo(null, contentType, photoData, filename);
            
            return await photoStore.AddPhotoAsync(photo);
        }

        private async Task<PhotoIdentifier> AddThumbnail(Stream photoData, string filename, string contentType, ThumbnailType thumbnailType)
        {
            var thumbnailPhotoData = await GeneratedThumbnailAsync(photoData, thumbnailType);
            
            var thumbnailPhoto = new Photo(
                null, 
                "image/jpg",
                thumbnailPhotoData,
                filename);
            
            return await photoStore.AddPhotoAsync(thumbnailPhoto);
        }

        private async Task<Stream> GeneratedThumbnailAsync([NotNull] Stream photoData, ThumbnailType thumbnailType)
        {
            if (photoData == null) throw new ArgumentNullException(nameof(photoData));

            var thumbnailConfiguration = ThumbnailConfigurations.Get(thumbnailType);

            var thumbnailData = new MemoryStream();
            await imageResizer.ResizeAsJpegAsync(
                photoData,
                thumbnailConfiguration.MaxWidth,
                thumbnailConfiguration.MaxHeight,
                thumbnailData);

            return thumbnailData;
        }
    }
}