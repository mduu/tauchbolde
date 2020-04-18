using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Driver.PhotoStorage.AzureBlobStorage;
using Tauchbolde.Driver.PhotoStorage.FileSystemStore;
using Tauchbolde.Tests.TestingTools;
using Tauchbolde.Web;
using Xunit;

namespace Tauchbolde.Tests.Web
{
    public class ApplicationServicesTests
    {
        private readonly IServiceCollection services = A.Fake<IServiceCollection>();
        private readonly IWebHostEnvironment hostingEnvironment = A.Fake<IWebHostEnvironment>();

        [Fact]
        public void TestRegisterWithoutPhotoStoreType()
        {
            var configuration = new ConfigurationMock();

            ApplicationServices.Register(services, configuration, hostingEnvironment);

            A.CallTo(() => services.Add(
                    A<ServiceDescriptor>.That.Matches(sd =>
                        sd.ServiceType == typeof(IPhotoStore) &&
                        sd.ImplementationType == typeof(FilePhotoStore))))
                .MustHaveHappenedOnceExactly();
        }
        
        [Theory]
        [InlineData("FileSystem", nameof(FilePhotoStore))]
        [InlineData("AzureBlobStorage", nameof(AzureBlobStore))]
        [InlineData("0", nameof(FilePhotoStore))]
        [InlineData("1", nameof(AzureBlobStore))]
        public void TestRegisterWithPhotoStoreType(string photoStoreTypeConfig, string expectedImplementation)
        {
            var configuration = new ConfigurationMock();
            configuration.Values["PhotoStoreType"] = photoStoreTypeConfig;

            ApplicationServices.Register(services, configuration, hostingEnvironment);

            A.CallTo(() => services.Add(
                    A<ServiceDescriptor>.That.Matches(sd =>
                        sd.ServiceType == typeof(IPhotoStore) &&
                        sd.ImplementationType.Name == expectedImplementation)))
                .MustHaveHappenedOnceExactly();
        }
    }
}