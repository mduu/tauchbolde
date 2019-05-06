using System.IO;
using JetBrains.Annotations;

namespace Tauchbolde.Common.DomainServices.PhotoStorage
{
    /// <summary>
    /// Metadata and binary data of a photo.
    /// </summary>
    public class Photo
    {
        public Photo(
            [NotNull] string contentType,
            [NotNull] Stream content,
            [CanBeNull] string filename)
        {
            ContentType = contentType;
            Content = content;
            Filename = filename;
        }
        
        /// <summary>
        /// The MIME content type of the <see cref="Content"/>.
        /// </summary>
        public string ContentType { get; }
        
        /// <summary>
        /// The binary content of the photo.
        /// </summary>
        public Stream Content { get; }
        
        /// <summary>
        /// The original filename of the photo.
        /// </summary>
        public string Filename { get; }
    }
}