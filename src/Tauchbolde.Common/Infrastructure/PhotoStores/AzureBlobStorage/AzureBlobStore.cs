using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Tauchbolde.Commom.Misc;
using Tauchbolde.Common.Domain.PhotoStorage;

namespace Tauchbolde.Common.Infrastructure.PhotoStores.AzureBlobStorage
{
    /// <summary>
    /// Implements a <see cref="IPhotoStore"/> that uses Azure Blog Storage
    /// to store the photo data.
    /// </summary>
    [UsedImplicitly]
    internal class AzureBlobStore : IPhotoStore
    {
        private const string ContainerName = "photos";
        private const string ThumbsPrefix = "thumbs";
        [NotNull] private readonly ILogger<AzureBlobStore> logger;
        [NotNull] private readonly CloudStorageAccount storageAccount;
        [NotNull] private readonly IMimeMapping mimeMapping;

        public AzureBlobStore(
            [NotNull] ILogger<AzureBlobStore> logger,
            [NotNull] IOptions<AzureBlobStoreConfiguration> configuration,
            [NotNull] IMimeMapping mimeMapping)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mimeMapping = mimeMapping ?? throw new ArgumentNullException(nameof(mimeMapping));
    
            if (!TryParseAzureBlobStorageAccount(
                configuration.Value.BlobStorageConnectionString,
                out var account))
            {
                logger.LogError(
                    $"{nameof(AzureBlobStore)}.ctor: Error parsing Azure Storage Account!",
                    configuration.Value.BlobStorageConnectionString);
                
                throw new ArgumentException($"Error parsing Azure Storage account: [{configuration.Value.BlobStorageConnectionString}]");
            }

            storageAccount = account;
        }

        /// <inheritdoc />
        public async Task AddPhotoAsync(Photo photo)
        {
            if (photo == null) throw new ArgumentNullException(nameof(photo));

            var cloudBlob = await GetCloudBlockBlob(photo.Identifier);
            await cloudBlob.UploadFromStreamAsync(photo.Content);
        }

        /// <inheritdoc />
        public async Task RemovePhotoAsync([NotNull] PhotoIdentifier photoIdentifier)
        {
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            var cloudBlob = await GetCloudBlockBlob(photoIdentifier);
            await cloudBlob.DeleteIfExistsAsync();
        }

        /// <inheritdoc />
        public async Task<Photo> GetPhotoAsync(PhotoIdentifier photoIdentifier)
        {
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            var cloudBlobContainer = await OpenOrCreateBlobContainerAsync();
            var cloudBlob = cloudBlobContainer.GetBlockBlobReference(GetBlobName(photoIdentifier));

            var memStream = new MemoryStream();
            await cloudBlob.DownloadToStreamAsync(memStream);
            await cloudBlob.FetchAttributesAsync();
            var contentType = cloudBlob.Properties?.ContentType ?? mimeMapping.GetMimeMapping(photoIdentifier.Filename);

            return new Photo(photoIdentifier, contentType, memStream);
        }

        private bool TryParseAzureBlobStorageAccount(
            [NotNull] string blobStorageConnectionString,
            out CloudStorageAccount cloudStorageAccount)
        {
            if (blobStorageConnectionString == null) throw new ArgumentNullException(nameof(blobStorageConnectionString));

            cloudStorageAccount = null;

            if (!CloudStorageAccount.TryParse(blobStorageConnectionString, out var account))
            {
                logger.LogError($"Error parsing Azure Storage account: [{blobStorageConnectionString}]");
                return false;
            }

            cloudStorageAccount = account;
            return true;
        }

        private async Task<CloudBlobContainer> OpenOrCreateBlobContainerAsync()
        {
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();

            var cloudBlobContainer = cloudBlobClient.GetContainerReference(ContainerName);
            var containerExists = await cloudBlobContainer.ExistsAsync();
            if (!containerExists)
            {
                await CreateBlobContainerAsync(cloudBlobContainer);
            }

            return cloudBlobContainer;
        }

        private async Task CreateBlobContainerAsync([NotNull] CloudBlobContainer cloudBlobContainer)
        {
            if (cloudBlobContainer == null) throw new ArgumentNullException(nameof(cloudBlobContainer));

            logger.LogInformation($"Creating Azure Blob Storage container with name [{cloudBlobContainer.Name}]");

            await cloudBlobContainer.CreateAsync();
            logger.LogInformation($"Azure Blob Storage container with name [{cloudBlobContainer.Name}] created successfully.");

            // Set the permissions so the blobs are public. 
            await cloudBlobContainer.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob,
                });
            
            logger.LogInformation($"Set permissions for Azure Blob Storage container with name [{cloudBlobContainer.Name}] successfully.");
        }


        private async Task<CloudBlockBlob> GetCloudBlockBlob([NotNull] PhotoIdentifier photoIdentifier)
        {
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            var cloudBlobContainer = await OpenOrCreateBlobContainerAsync();
            var result = cloudBlobContainer.GetBlockBlobReference(GetBlobName(photoIdentifier));

            return result;
        }

        private static string GetBlobName(PhotoIdentifier photoIdentifier)
        {
            return photoIdentifier.IsThumb
                ? $"{photoIdentifier.Category.ToString()}/{ThumbsPrefix}/{photoIdentifier.Filename}"
                : $"{photoIdentifier.Category.ToString()}/{photoIdentifier.Filename}";
        }
    }
}