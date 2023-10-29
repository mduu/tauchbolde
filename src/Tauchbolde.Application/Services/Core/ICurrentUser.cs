using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.Services.Core
{
    public interface ICurrentUser
    {
        string Username { get; }
        Task<bool> GetIsAdminAsync();
        Task<bool> GetIsTauchboldAsync();
        Task<Diver> GetCurrentDiverAsync();
        Task<bool> GetIsTauchboldOrAdminAsync();
        Task<bool> GetIsDiverOrAdmin(Guid diverId);
    }
}