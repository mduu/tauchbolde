using JetBrains.Annotations;

namespace Tauchbolde.Common.Domain.PhotoStorage.Stores.FileSystemStore
{
    public interface IFilePathCalculator
    {
        string CalculateUniqueFilePath(
            string rootPath, 
            PhotoCategory category, 
            [CanBeNull] string baseFileName, 
            [CanBeNull] string contentType,
            ThumbnailType thumbnailType = ThumbnailType.None);

        string CalculatePath(
            [NotNull] string rootPath, 
            [NotNull] FilePhotoIdentifierInfos photoIdentifierInfos);
    }
}