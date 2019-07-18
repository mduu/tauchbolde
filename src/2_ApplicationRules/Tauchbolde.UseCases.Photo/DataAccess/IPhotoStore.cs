using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Entities;

namespace Tauchbolde.UseCases.Photo.DataAccess
{
    public interface IPhotoStore
    {
        Task AddPhotoAsync([NotNull] Entities.Photo photo);
        Task RemovePhotoAsync(PhotoIdentifier photoIdentifier);
        Task<Entities.Photo> GetPhotoAsync([NotNull] PhotoIdentifier photoIdentifier);
    }
}