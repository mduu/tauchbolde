using SixLabors.ImageSharp.Formats;
using Tauchbolde.Application.Services;

namespace Tauchbolde.Driver.ImageSharp
{
    /// <summary>
    /// Implements a <see cref="IImageResizer"/> using ImageSharp library.
    /// </summary>
    public class ImageResizer : IImageResizer
    {
        /// <inheritdoc/>
        public Stream Resize(
            int maxWidth,
            int maxHeight,
            Stream imageData,
            string targetFileExt,
            string contentType = null)
        {
            var outStream = new MemoryStream();

            DecoderOptions options = new()
            {
                MaxFrames = 1,
                TargetSize = new(maxWidth, maxHeight)
            };

            var image = Image.Load(options, imageData);

            using (image)
            {
                switch (targetFileExt.ToLower())
                {
                    case ".jpg":
                    case ".jpeg":
                        image.SaveAsJpeg(outStream);
                        break;
                    case ".png":
                        image.SaveAsPng(outStream);
                        break;
                    case ".gif":
                        image.SaveAsGif(outStream);
                        break;
                    default:
                        throw new InvalidOperationException($"File extension [{targetFileExt}] not supported!");
                }
            }

            outStream.Seek(0, 0);
            return outStream;
        }
    }
}