using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Users
{
    public interface IDiverService
    {
        Task UpdateRolesAsync(Diver member, ICollection<string> roles);
    }
}