using JetBrains.Annotations;

namespace Tauchbolde.Common.Domain.PhotoStorage.Stores.FileSystemStore
{
    public interface IFilePhotoIdentifierSerializer
    {
        [NotNull] PhotoIdentifier SerializePhotoIdentifier([NotNull] FilePhotoIdentifierInfos filePhotoIdentifierInfos);
        [NotNull] FilePhotoIdentifierInfos DeserializePhotoIdentifier([NotNull] PhotoIdentifier photoIdentifier);
    }
}