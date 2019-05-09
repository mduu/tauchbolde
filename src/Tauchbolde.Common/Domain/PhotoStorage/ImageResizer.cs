using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Tauchbolde.Common.Domain.PhotoStorage
{
    public class ImageResizer : IImageResizer
    {
        /// <inheritdoc />
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task ResizeAsJpegAsync(Stream originalContent, int width, int height, Stream outputStream)

        {
            if (originalContent == null) throw new ArgumentNullException(nameof(originalContent));

            using (var image = Image.Load(originalContent))
            {
                image.Mutate(x => x
                     .Resize(width, height));

                image.SaveAsJpeg(outputStream);
            }
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
