using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Tauchbolde.Common.DomainServices.PhotoStorage.Stores.FileSystemStore
{
    public class FilePhotoStore : IPhotoStore
    {
        [NotNull] private readonly ILogger<FilePhotoStore> logger;
        private readonly IFilePhotoStoreConfiguration configuration;
        private readonly IFilePathCalculator filePathCalculator;
        [NotNull] private readonly IFilePhotoIdentifierSerializer identifierSerializer;

        public FilePhotoStore(
            [NotNull] ILogger<FilePhotoStore> logger,
            [NotNull] IFilePhotoStoreConfiguration configuration,
            [NotNull] IFilePathCalculator filePathCalculator,
            [NotNull] IFilePhotoIdentifierSerializer identifierSerializer)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.filePathCalculator = filePathCalculator ?? throw new ArgumentNullException(nameof(filePathCalculator));
            this.identifierSerializer =
                identifierSerializer ?? throw new ArgumentNullException(nameof(identifierSerializer));
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

            var photoIdentifierPInfos = identifierSerializer.DeserializePhotoIdentifier(photoIdentifier);
            var filePath = filePathCalculator.CalculatePath(configuration.RootFolder, photoIdentifierPInfos);

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

            // TODO Identifier to path

            // TODO Build and return photo

            throw new NotImplementedException();
        }

        internal PhotoIdentifier BuildPhotoIdentifier(
            PhotoCategory category,
            string filename)
        {
            // TODO Build a unique PhotoIdentifier from the given arguments

            throw new NotImplementedException();
        }
    }
}