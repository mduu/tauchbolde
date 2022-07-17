using JetBrains.Annotations;
using Tauchbolde.Domain.ValueObjects;

namespace Tauchbolde.Application.Services.PhotoStores
{
    public interface IPhotoStore
    {
        Task AddPhotoAsync([NotNull] Photo photo);
        Task RemovePhotoAsync(PhotoIdentifier photoIdentifier);
        Task<Photo> GetPhotoAsync([NotNull] PhotoIdentifier photoIdentifier);
    }
}