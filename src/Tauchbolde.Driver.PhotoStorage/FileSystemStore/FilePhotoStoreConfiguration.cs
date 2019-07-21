using System;
using JetBrains.Annotations;

namespace Tauchbolde.Driver.PhotoStorage.FileSystemStore
{
    public class FilePhotoStoreConfiguration : IFilePhotoStoreConfiguration
    {
        public string RootFolder { get; }

        public FilePhotoStoreConfiguration([NotNull] string rootFolder)
        {
            RootFolder = rootFolder ?? throw new ArgumentNullException(nameof(rootFolder));
        }
    }
}