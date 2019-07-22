using JetBrains.Annotations;
using Tauchbolde.Domain.Types;
using Tauchbolde.Domain.ValueObjects;

namespace Tauchbolde.Driver.PhotoStorage.FileSystemStore
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