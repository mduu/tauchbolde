using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Driver.PhotoStorage.AzureBlobStorage;
using Tauchbolde.Driver.PhotoStorage.FileSystemStore;

[assembly: InternalsVisibleTo("Tauchbolde.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] // For FakeItEasy to use "internal" visibility

namespace Tauchbolde.Driver.PhotoStorage
{
    public static class PhotoStorageRegistration
    {
        public static void RegisterServices(
            [NotNull] IServiceCollection services,
            string photoStoreRoot,
            PhotoStoreType photoStoreType)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddSingleton<IFilePhotoStoreConfiguration>(new FilePhotoStoreConfiguration(photoStoreRoot));
            services.AddTransient<IFilePathCalculator, FilePathCalculator>();
            switch (photoStoreType)
            {
                case PhotoStoreType.AzureBlobStorage:
                    services.AddTransient<IPhotoStore, AzureBlobStore>();
                    break;
                default:
                    services.AddTransient<IPhotoStore, FilePhotoStore>();
                    break;
            }
        }
    }
}