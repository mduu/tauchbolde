using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.Services.Core
{
    internal interface ICurrentUser
    {
        Task<Diver> GetCurrentDiver();
    }
}