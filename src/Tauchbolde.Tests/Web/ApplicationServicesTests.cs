using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Common.Domain.PhotoStorage;
using Tauchbolde.Common.Infrastructure.PhotoStores.AzureBlobStorage;
using Tauchbolde.Common.Infrastructure.PhotoStores.FileSystemStore;
using Tauchbolde.Tests.TestingTools;
using Tauchbolde.UseCases.Photo.DataAccess;
using Tauchbolde.Web;
using Xunit;

namespace Tauchbolde.Tests.Web
{
    public class ApplicationServicesTests
    {
        private readonly IServiceCollection services = A.Fake<IServiceCollection>();
        private readonly IHostingEnvironment hostingEnvironment = A.Fake<IHostingEnvironment>();

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