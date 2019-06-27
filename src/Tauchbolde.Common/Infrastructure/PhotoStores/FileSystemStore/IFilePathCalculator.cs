using JetBrains.Annotations;
using Tauchbolde.Common.Domain.PhotoStorage;

namespace Tauchbolde.Common.Infrastructure.PhotoStores.FileSystemStore
{
    public interface IFilePathCalculator
    {
        string CalculateUniqueFilePath(
            string rootPath, 
            PhotoCategory category, 
            [CanBeNull] string baseFileName, 
            [CanBeNull] string contentType,
            bool isThumb);

        string CalculatePath(
            [NotNull] string rootPath, 
            [NotNull] PhotoIdentifier photoIdentifier);
    }
}