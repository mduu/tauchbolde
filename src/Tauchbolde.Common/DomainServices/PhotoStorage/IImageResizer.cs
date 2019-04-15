using System.IO;
using System.Threading.Tasks;

namespace Tauchbolde.Common.DomainServices.PhotoStorage
{
    /// <summary>
    /// Interface for generating thumbnails from an original image/asset.
    /// </summary>
    public interface IImageResizer
    {
        Task ResizeAsJpegAsync(Stream originalContent, int width, int height, Stream outputStream);
    }
}
