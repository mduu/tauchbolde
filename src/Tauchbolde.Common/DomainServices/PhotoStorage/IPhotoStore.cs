using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tauchbolde.Common.DomainServices.PhotoStorage
{
    public interface IPhotoStore
    {
        Task<PhotoIdentifier> AddPhotoAsync(PhotoCategory category, [NotNull] Photo photo, ThumbnailType thumbnailType = ThumbnailType.None);
        Task RemovePhotoAsync([NotNull] PhotoIdentifier photoIdentifier);
        Task<Photo> GetPhotoAsync([NotNull] PhotoIdentifier photoIdentifier);
    }
}