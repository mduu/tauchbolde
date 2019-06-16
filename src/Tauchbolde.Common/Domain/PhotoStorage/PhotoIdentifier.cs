using System;
using JetBrains.Annotations;

namespace Tauchbolde.Common.Domain.PhotoStorage
{
    /// <summary>
    /// Represents a unique photo identifier.
    /// </summary>
    public class PhotoIdentifier
    {
        public PhotoIdentifier([NotNull] string identifier)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

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

            return photoIdentifierObj.ToString()?.Equals(ToString()) ?? false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Identifier ?? "";
        }
    }
}