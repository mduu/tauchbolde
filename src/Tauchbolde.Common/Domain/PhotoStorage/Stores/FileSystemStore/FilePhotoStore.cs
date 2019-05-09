using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Commom.Misc;

namespace Tauchbolde.Common.Domain.PhotoStorage.Stores.FileSystemStore
{
    public class FilePhotoStore : IPhotoStore
    {
        [NotNull] private readonly ILogger<FilePhotoStore> logger;
        [NotNull] private readonly IFilePhotoStoreConfiguration configuration;
        [NotNull] private readonly IFilePathCalculator filePathCalculator;
        [NotNull] private readonly IFilePhotoIdentifierSerializer identifierSerializer;
        [NotNull] private readonly IMimeMapping mimeMapping;

        public FilePhotoStore(
            [NotNull] ILogger<FilePhotoStore> logger,
            [NotNull] IFilePhotoStoreConfiguration configuration,
            [NotNull] IFilePathCalculator filePathCalculator,
            [NotNull] IFilePhotoIdentifierSerializer identifierSerializer,
            [NotNull] IMimeMapping mimeMapping)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.filePathCalculator = filePathCalculator ?? throw new ArgumentNullException(nameof(filePathCalculator));
            this.identifierSerializer =
                identifierSerializer ?? throw new ArgumentNullException(nameof(identifierSerializer));
            this.mimeMapping = mimeMapping ?? throw new ArgumentNullException(nameof(mimeMapping));
        }

        public async Task<PhotoIdentifier> AddPhotoAsync(
            PhotoCategory category,
            Photo photo,
            ThumbnailType thumbnailType = ThumbnailType.None)
        {
            if (photo == null) throw new ArgumentNullException(nameof(photo));

            var photoFilePath = filePathCalculator.CalculateUniqueFilePath(
                configuration.RootFolder,
                category,
                photo.Filename,
                photo.ContentType,
                thumbnailType
            );

            using (var fileStream = new FileStream(photoFilePath, FileMode.CreateNew, FileAccess.Write))
            {
                await photo.Content.CopyToAsync(fileStream);
            }

            return identifierSerializer.SerializePhotoIdentifier(
                new FilePhotoIdentifierInfos(
                    category, thumbnailType, Path.GetFileName(photoFilePath)
                )
            );
        }

        public async Task RemovePhotoAsync(PhotoIdentifier photoIdentifier)
        {
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            var filePath = filePathCalculator
                .CalculatePath(
                    configuration.RootFolder,
                    identifierSerializer.DeserializePhotoIdentifier(photoIdentifier));

            if (!File.Exists(filePath))
            {
                logger.LogWarning(
                    $"Photo file can not be deleted from disk because it does not exists! File: [{filePath}]",
                    photoIdentifier);

                return;
            }

            try
            {
                File.Delete(filePath);
            }
            catch (IOException ex)
            {
                logger.LogError($"Error deleting photo file [{filePath}] from disk!", ex, photoIdentifier);
            }
        }

        public async Task<Photo> GetPhotoAsync(PhotoIdentifier photoIdentifier)
        {
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            var filePhotoIdentifierInfos = identifierSerializer.DeserializePhotoIdentifier(photoIdentifier);
            var filePath = filePathCalculator
                .CalculatePath(
                    configuration.RootFolder,
                    filePhotoIdentifierInfos);


            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var memStream = new MemoryStream();
                await fileStream.CopyToAsync(memStream);

                return new Photo(
                    mimeMapping.GetMimeMapping(
                        Path.GetExtension(filePhotoIdentifierInfos.Filename)
                    ),
                    memStream,
                    filePhotoIdentifierInfos.Filename);
            }
        }
    }
}