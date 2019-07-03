using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Common.Infrastructure;

namespace Tauchbolde.Common.Domain.PhotoStorage
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
        
        public async Task<PhotoAndThumbnailIdentifiers> AddPhotoAsync(
            PhotoCategory category,
            Stream photoData,
            string contentType,
            string filename)
        {
            if (photoData == null) throw new ArgumentNullException(nameof(photoData));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            
            return await AddPhotoWithThumbnail(category, photoData, filename, contentType);
        }
        
        public async Task<PhotoAndThumbnailIdentifiers> UpdatePhotoAsync(
            PhotoAndThumbnailIdentifiers existingPhoto,
            PhotoCategory category,
            Stream photoData,
            string filename,
            string contentType)
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

            return await AddPhotoWithThumbnail(category, photoData, filename, contentType);
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
        
        private async Task<PhotoAndThumbnailIdentifiers> AddPhotoWithThumbnail(PhotoCategory photoCategory,
            [NotNull] Stream photoData,
            string filename,
            string contentType)
        {
            if (photoData == null) throw new ArgumentNullException(nameof(photoData));

            return new PhotoAndThumbnailIdentifiers(
                await AddOriginalPhoto(photoData, filename, contentType, photoCategory),
                await AddThumbnail(photoData, filename, contentType, photoCategory));
        }

        private async Task<PhotoIdentifier> AddOriginalPhoto(
            [NotNull] Stream photoData,
            string filename,
            string contentType,
            PhotoCategory photoCategory)
        {
            if (photoData == null) throw new ArgumentNullException(nameof(photoData));

            var photoIdentifier = new PhotoIdentifier(photoCategory, false, filename);
            
            await photoStore.AddPhotoAsync(
                new Photo(
                    photoIdentifier,
                    contentType,
                    photoData));

            return photoIdentifier;
        }

        private async Task<PhotoIdentifier> AddThumbnail(
            [NotNull] Stream photoData,
            string filename,
            string contentType,
            PhotoCategory photoCategory)
        {
            if (photoData == null) throw new ArgumentNullException(nameof(photoData));
            
            var thumbnailPhotoData = await GeneratedThumbnailAsync(photoData, photoCategory, contentType);
            var photoIdentifier = new PhotoIdentifier(photoCategory, true, filename);
            
            await photoStore.AddPhotoAsync(
                new Photo(
                    photoIdentifier, 
                    "image/jpg",
                    thumbnailPhotoData));

            return photoIdentifier;
        }

        private async Task<Stream> GeneratedThumbnailAsync(
            [NotNull] Stream photoData,
            PhotoCategory photoCategory,
            string contentType)
        {
            if (photoData == null) throw new ArgumentNullException(nameof(photoData));

            var thumbnailConfiguration = PhotoCategoryConfig.Configs[photoCategory];

            var thumbnailData = imageResizer.Resize(
                thumbnailConfiguration.ThumbMaxWidth,
                thumbnailConfiguration.ThumbMaxHeight,
                photoData,
                ".jpg",
                contentType);

            return thumbnailData;
        }
    }
}