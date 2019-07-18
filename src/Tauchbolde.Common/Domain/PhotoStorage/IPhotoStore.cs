using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Entities;

namespace Tauchbolde.Common.Domain.PhotoStorage
{
    public interface IPhotoStore
    {
        Task AddPhotoAsync([NotNull] Photo photo);
        Task RemovePhotoAsync(PhotoIdentifier photoIdentifier);
        Task<Photo> GetPhotoAsync([NotNull] PhotoIdentifier photoIdentifier);
    }
}