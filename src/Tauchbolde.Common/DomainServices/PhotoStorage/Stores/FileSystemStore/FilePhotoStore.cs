using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tauchbolde.Common.DomainServices.PhotoStorage.Stores.FileSystemStore
{
    public class FilePhotoStore : IPhotoStore
    {
        private readonly IFilePhotoStoreConfiguration configuration;
        private readonly IFilePathCalculator filePathCalculator;
        [NotNull] private readonly IFilePhotoIdentifierSerializer identifierSerializer;

        public FilePhotoStore(
            [NotNull] IFilePhotoStoreConfiguration configuration,
            [NotNull] IFilePathCalculator filePathCalculator,
            [NotNull] IFilePhotoIdentifierSerializer identifierSerializer)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.filePathCalculator = filePathCalculator ?? throw new ArgumentNullException(nameof(filePathCalculator));
            this.identifierSerializer = identifierSerializer ?? throw new ArgumentNullException(nameof(identifierSerializer));
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

            // TODO Identifier to path
            
            // TODO Delete file
            
            throw new NotImplementedException();
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