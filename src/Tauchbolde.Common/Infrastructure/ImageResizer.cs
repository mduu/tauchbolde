using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Tauchbolde.Common.Infrastructure
{
    /// <summary>
    /// Implements a <see cref="IImageResizer"/> using ImageSharp library.
    /// </summary>
    public class ImageResizer : IImageResizer
    {
        /// <inheritdoc/>
        public Stream Resize(int maxWidth, int maxHeight, Stream imageData, string fileExt)
        {
            var outStream = new MemoryStream();
            using (var image = Image.Load(imageData))
            {
                image.Mutate(x => x
                    .Resize(maxWidth, maxHeight)
                );

                switch (fileExt.ToLower())
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
                        throw new InvalidOperationException($"File extension [{fileExt}] not supported!");
                }
            }

            outStream.Seek(0, 0);
            return outStream;
        }
    }
}
