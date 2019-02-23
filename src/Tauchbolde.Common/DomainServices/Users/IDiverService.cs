using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.DataAccess;

namespace Tauchbolde.Common.DomainServices.Users
{
    public interface IDiverService
    {
        Task<ICollection<Diver>> GetMembersAsync(IDiverRepository diverRepository);
        Task<Diver> GetMemberAsync(IDiverRepository diverRepository, string userName);
        Task<Diver> GetMemberAsync(IDiverRepository diverRepository, Guid diverId);
        Task UpdateUserProfilAsync(IDiverRepository diverRepository, Diver profile);
        Task UpdateRolesAsync(Diver member, ICollection<string> roles);
        Task<string> AddMembersAsync(IDiverRepository diverRepository, string userName, string firstname, string lastname);
    }
}