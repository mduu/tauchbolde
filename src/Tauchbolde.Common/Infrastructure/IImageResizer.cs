using System.IO;

namespace Tauchbolde.Common.Infrastructure
{
    /// <summary>
    /// Interface for resizing images.
    /// </summary>
    public interface IImageResizer
    {
        /// <summary>
        /// Resize the image in <paramref name="imageData"/> so it is not larger
        /// then <paramref name="maxWidth"/> and <paramref name="maxHeight"/>.
        /// </summary>
        /// <param name="maxWidth">Max width in pixels.</param>
        /// <param name="maxHeight">Max height in pixels.</param>
        /// <param name="imageData">Binary image data.</param>
        /// <param name="fileExt">File extension (eg. ",jpg").</param>
        Stream Resize(int maxWidth, int maxHeight, Stream imageData, string fileExt);
    }
}