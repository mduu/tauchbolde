using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tauchbolde.Application.Services;
using Tauchbolde.Domain.Types;
using Tauchbolde.Domain.ValueObjects;
using Tauchbolde.Driver.PhotoStorage.AzureBlobStorage;
using Xunit;

namespace Tauchbolde.Tests.Drivers.PhotoStores.AzureBlobStorage
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
            var photo = CreateSamplePhoto();

            await store.AddPhotoAsync(photo);

            A.CallTo(() => logger.Log(LogLevel.Error, A<EventId>._, A<object>._, A<Exception>._,
                    A<Func<object, Exception, string>>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task Test_GetPhoto()
        {
            var store = CreateStore();
            var photo = CreateSamplePhoto();
            await store.AddPhotoAsync(photo);

            var result = await store.GetPhotoAsync(photo.Identifier);

            result.ContentType.Should().Be("image/jpeg");
            result.Content.Length.Should().Be(photo.Content.Length);
        }

        private static Photo CreateSamplePhoto()
        {
            var identifier = new PhotoIdentifier(PhotoCategory.LogbookTeaser, false, "test.jpg");
            var contentType = "image/jpeg";
            Stream content = new MemoryStream(new byte[] {42, 21, 1});
            
            return new Photo(identifier, contentType, content);
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
                new MimeMapping(),
                A.Fake<ITelemetryService>());
        }
    }
}