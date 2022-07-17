using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
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

            var imageDecoder = GetImageDecoder(contentType);

            imageData.Position = 0;
            var image = imageDecoder != null
                ? Image.Load(imageData, imageDecoder)
                : Image.Load(imageData);

            using (image)
            {
                image.Mutate(x => x
                    .Resize(maxWidth, 0)
                );

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

        private static IImageDecoder GetImageDecoder(string contentType) =>
            string.IsNullOrWhiteSpace(contentType)
                ? null
                : contentType switch
                {
                    "image/png" => new PngDecoder(),
                    "image/gif" => new GifDecoder(),
                    "image/jpg" => new JpegDecoder(),
                    "image/jpeg" => new JpegDecoder(),
                    _ => throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null)
                };
    }
}