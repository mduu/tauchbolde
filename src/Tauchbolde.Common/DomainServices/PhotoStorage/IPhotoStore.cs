using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tauchbolde.Common.DomainServices.PhotoStorage
{
    public interface IPhotoStore
    {
        Task<PhotoIdentifier> AddPhotoAsync([NotNull] Photo photo);
        Task RemovePhotoAsync([NotNull] PhotoIdentifier photoIdentifier);
        Task<Photo> GetPhotoAsync([NotNull] PhotoIdentifier photoIdentifier);
    }
}