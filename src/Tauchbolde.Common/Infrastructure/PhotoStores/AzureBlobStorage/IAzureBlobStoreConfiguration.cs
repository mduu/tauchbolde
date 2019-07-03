namespace Tauchbolde.Common.Infrastructure.PhotoStores.AzureBlobStorage
{
    public interface IAzureBlobStoreConfiguration
    {
        string BlobStorageConnectionString { get; set; }
    }
}