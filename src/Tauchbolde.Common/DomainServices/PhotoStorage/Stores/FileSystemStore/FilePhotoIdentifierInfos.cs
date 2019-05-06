using System;
using JetBrains.Annotations;

namespace Tauchbolde.Common.DomainServices.PhotoStorage.Stores.FileSystemStore
{
    /// <summary>
    /// Contains the parts that build up a <see cref="PhotoIdentifier"/> when using
    /// the file based photo store (see <see cref="FilePhotoStore"/>).
    /// </summary>
    /// <seealso cref="PhotoIdentifier"/>
    /// <seealso cref="FilePhotoStore"/>
    public class FilePhotoIdentifierInfos
    {
        public FilePhotoIdentifierInfos(PhotoCategory category, ThumbnailType thumbnailType, [NotNull] string filename)
        {
            Category = category;
            ThumbnailType = thumbnailType;
            Filename = filename ?? throw new ArgumentNullException(nameof(filename));
        }
        
        public PhotoCategory Category { get; }
        public ThumbnailType ThumbnailType { get; }
        public string Filename { get; }
    }
}