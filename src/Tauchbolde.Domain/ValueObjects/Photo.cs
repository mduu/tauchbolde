using System;
using System.IO;
using JetBrains.Annotations;
using Tauchbolde.Domain.SharedKernel;

namespace Tauchbolde.Domain.ValueObjects
{
    /// <summary>
    /// Metadata and binary data of a photo.
    /// </summary>
    /// <seealso cref="PhotoIdentifier"/>
    public class Photo : ValueObject
    {
        public Photo(
            [NotNull] PhotoIdentifier identifier,
            [NotNull] string contentType,
            [NotNull] Stream content)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
        
        /// <summary>
        /// Identifies the photo with category, filename and an indicator
        /// whether it is a thumbnail or not.
        /// </summary>
        /// <seealso cref="PhotoIdentifier"/>
        public PhotoIdentifier Identifier { get; set; }
        
        /// <summary>
        /// The MIME content type of the <see cref="Content"/>.
        /// </summary>
        public string ContentType { get; }
        
        /// <summary>
        /// The binary content of the photo.
        /// </summary>
        [NotNull, IgnoreMember] public Stream Content { get; }
    }
}