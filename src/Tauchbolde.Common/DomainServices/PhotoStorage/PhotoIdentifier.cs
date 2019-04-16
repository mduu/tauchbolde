using System;
using JetBrains.Annotations;

namespace Tauchbolde.Common.DomainServices.PhotoStorage
{
    /// <summary>
    /// Represents a unique photo identifier.
    /// </summary>
    public class PhotoIdentifier
    {
        public PhotoIdentifier(PhotoCategory category)
        {
            Category = category;
        }
        
        public PhotoIdentifier(PhotoCategory category, [NotNull] string identifier)
        {
            Category = category;
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        /// <summary>
        /// The category of the photo which can act as a namespace when storing photos.
        /// </summary>
        public PhotoCategory Category { get; }

        /// <summary>
        /// Unique identifier of a photo.
        /// </summary>
        public string Identifier { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is PhotoIdentifier photoIdentifierObj))
            {
                return false;
            }

            return photoIdentifierObj.ToString()?.Equals(this.ToString()) ?? false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Category}:{Identifier}" ?? "";
        }
    }
}