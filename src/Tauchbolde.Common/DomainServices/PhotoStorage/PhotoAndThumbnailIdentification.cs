using JetBrains.Annotations;

namespace Tauchbolde.Common.DomainServices.PhotoStorage
{
    public class PhotoAndThumbnailIdentification
    {
        public PhotoAndThumbnailIdentification(
            [CanBeNull] PhotoIdentifier originalPhotoIdentifier,
            [CanBeNull] PhotoIdentifier thumbnailPhotoIdentifier)
        {
            OriginalPhotoIdentifier = originalPhotoIdentifier;
            ThumbnailPhotoIdentifier = thumbnailPhotoIdentifier;
        }
        
        [CanBeNull] public PhotoIdentifier OriginalPhotoIdentifier { get; }
        [CanBeNull] public PhotoIdentifier ThumbnailPhotoIdentifier { get; }
    }
}