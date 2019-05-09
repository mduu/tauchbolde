using System;
using System.Text.RegularExpressions;

namespace Tauchbolde.Common.Domain.PhotoStorage.Stores.FileSystemStore
{
    internal class FilePhotoIdentifierSerializer : IFilePhotoIdentifierSerializer
    {
        private static readonly Regex ParseRegex = new Regex(@"^(?<category>.*)\/(?<thumbnail>.*)\/(?<filename>.*)$", RegexOptions.Compiled);
        
        public PhotoIdentifier SerializePhotoIdentifier(FilePhotoIdentifierInfos filePhotoIdentifierInfos)
        {
            if (filePhotoIdentifierInfos == null) throw new ArgumentNullException(nameof(filePhotoIdentifierInfos));

            return new PhotoIdentifier(
                $"{filePhotoIdentifierInfos.Category.ToString()}/{filePhotoIdentifierInfos.ThumbnailType.ToString()}{filePhotoIdentifierInfos.Filename}"
            );
        }

        public FilePhotoIdentifierInfos DeserializePhotoIdentifier(PhotoIdentifier photoIdentifier)
        {
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            var match = ParseRegex.Match(photoIdentifier.Identifier);

            if (!match.Success)
            {
                throw new InvalidOperationException($"Invalid PhotoIdentifier: [${photoIdentifier.Identifier}]");
            }

            var categoryName = match.Groups["category"].Value;
            if (!Enum.TryParse(categoryName, out PhotoCategory category))
            {
                throw new InvalidOperationException($"Invalid PhotoIdentifier: [${photoIdentifier.Identifier}]");
            }

            var thumbnailTypeName = match.Groups["thumbnail"].Value;
            if (!Enum.TryParse(thumbnailTypeName, out ThumbnailType thumbnailType))
            {
                throw new InvalidOperationException($"Invalid PhotoIdentifier: [${photoIdentifier.Identifier}]");
            }

            var filename = match.Groups["filename"].Value;
                
            return new FilePhotoIdentifierInfos(category, thumbnailType, filename);
        }
    }
}