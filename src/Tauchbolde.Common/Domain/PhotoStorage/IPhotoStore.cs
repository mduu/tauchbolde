using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tauchbolde.Common.Domain.PhotoStorage
{
    public interface IPhotoStore
    {
        Task<PhotoIdentifier> AddPhotoAsync([NotNull] Photo photo);
        Task RemovePhotoAsync(PhotoIdentifier photoIdentifier);
        Task<Photo> GetPhotoAsync([NotNull] PhotoIdentifier photoIdentifier);
    }
}