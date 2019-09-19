using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.Services.Core
{
    internal interface ICurrentUser
    {
        string Username { get; }
        Task<Diver> GetCurrentDiverAsync();
    }
}