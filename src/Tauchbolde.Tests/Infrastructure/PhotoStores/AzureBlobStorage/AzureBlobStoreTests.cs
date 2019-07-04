using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tauchbolde.Commom.Misc;
using Tauchbolde.Common.Domain.PhotoStorage;
using Tauchbolde.Common.Infrastructure.PhotoStores.AzureBlobStorage;
using Xunit;

namespace Tauchbolde.Tests.Infrastructure.PhotoStores.AzureBlobStorage
{
    public class AzureBlobStoreTests
    {
        private readonly ILogger<AzureBlobStore> logger;
        private readonly string azureBlobConnectionString;

        public AzureBlobStoreTests()
        {
            azureBlobConnectionString = Environment.GetEnvironmentVariable("tb_storage_connectionstring");

            logger = A.Fake<ILogger<AzureBlobStore>>();
        }

        [Fact]
        public async Task Test_AddPhoto()
        {
            var store = CreateStore();
            var identifier = new PhotoIdentifier(PhotoCategory.LogbookTeaser, false, "test.jpg");
            var contentType = "image/jpg";
            Stream content = new MemoryStream(new byte[] {42, 21, 1});
            var photo = new Photo(identifier, contentType, content);

            await store.AddPhotoAsync(photo);

            A.CallTo(() => logger.Log<object>(LogLevel.Error, A<EventId>._, A<object>._, A<Exception>._,
                    A<Func<object, Exception, string>>._))
                .MustNotHaveHappened();
        }

        private AzureBlobStore CreateStore(AzureBlobStoreConfiguration configuration = null)
        {
            return new AzureBlobStore(
                logger,
                Options.Create(
                    configuration ?? new AzureBlobStoreConfiguration
                    {
                        BlobStorageConnectionString = azureBlobConnectionString
                    }),
                new MimeMapping());
        }
    }
}