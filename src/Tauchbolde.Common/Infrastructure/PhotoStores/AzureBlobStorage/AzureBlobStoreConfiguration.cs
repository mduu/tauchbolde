namespace Tauchbolde.Common.Infrastructure.PhotoStores.AzureBlobStorage
{
    public class AzureBlobStoreConfiguration : IAzureBlobStoreConfiguration
    {
        public string BlobStorageConnectionString { get; set; }
    }
}