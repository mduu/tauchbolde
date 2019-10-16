using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Users
{
    public interface IDiverService
    {
        Task<Diver> GetMemberAsync(Guid diverId);
        Task UpdateRolesAsync(Diver member, ICollection<string> roles);
        Task<string> AddMembersAsync(string userName, string firstname, string lastname);
    }
}