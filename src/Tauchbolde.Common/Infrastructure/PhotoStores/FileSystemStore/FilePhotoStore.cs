using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Commom.Misc;
using Tauchbolde.Common.Domain.PhotoStorage;
using Tauchbolde.Entities;
using Tauchbolde.UseCases.Photo.DataAccess;

namespace Tauchbolde.Common.Infrastructure.PhotoStores.FileSystemStore
{
    public class FilePhotoStore : IPhotoStore
    {
        [NotNull] private readonly ILogger<FilePhotoStore> logger;
        [NotNull] private readonly IFilePhotoStoreConfiguration configuration;
        [NotNull] private readonly IFilePathCalculator filePathCalculator;
        [NotNull] private readonly IMimeMapping mimeMapping;

        /// <inheritdoc />
        public FilePhotoStore(
            [NotNull] ILogger<FilePhotoStore> logger,
            [NotNull] IFilePhotoStoreConfiguration configuration,
            [NotNull] IFilePathCalculator filePathCalculator,
            [NotNull] IMimeMapping mimeMapping)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.filePathCalculator = filePathCalculator ?? throw new ArgumentNullException(nameof(filePathCalculator));
            this.mimeMapping = mimeMapping ?? throw new ArgumentNullException(nameof(mimeMapping));
        }

        /// <inheritdoc />
        public async Task AddPhotoAsync(Photo photo)
        {
            if (photo == null) throw new ArgumentNullException(nameof(photo));

            var photoFilePath = filePathCalculator.CalculateUniqueFilePath(
                configuration.RootFolder,
                photo.Identifier.Category,
                photo.Identifier.Filename,
                photo.ContentType,
                photo.Identifier.IsThumb
            );

            var directoryPath = Path.GetDirectoryName(photoFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var fileStream = new FileStream(photoFilePath, FileMode.CreateNew, FileAccess.Write))
            {
                photo.Content.Seek(0, 0);
                await photo.Content.CopyToAsync(fileStream);
            }
        }

        /// <inheritdoc />
        public async Task RemovePhotoAsync(PhotoIdentifier photoIdentifier)
        {
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            var filePath = filePathCalculator
                .CalculatePath(
                    configuration.RootFolder,
                    photoIdentifier);

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

        /// <inheritdoc />
        public async Task<Photo> GetPhotoAsync(PhotoIdentifier photoIdentifier)
        {
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            var filePath = filePathCalculator.CalculatePath(configuration.RootFolder, photoIdentifier);


            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var memStream = new MemoryStream();
                await fileStream.CopyToAsync(memStream);
                memStream.Seek(0, 0);

                return new Photo(
                    photoIdentifier,
                    mimeMapping.GetMimeMapping(Path.GetExtension(photoIdentifier.Filename)),
                    memStream);
            }
        }
    }
}