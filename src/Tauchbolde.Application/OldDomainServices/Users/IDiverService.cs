using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Users
{
    public interface IDiverService
    {
        Task<Diver> FindByUserNameAsync(string username);
        Task<ICollection<Diver>> GetMembersAsync();
        Task<Diver> GetMemberAsync(string userName);
        Task<Diver> GetMemberAsync(Guid diverId);
        Task UpdateUserProfileAsync(Diver profile);
        Task UpdateRolesAsync(Diver member, ICollection<string> roles);
        Task<string> AddMembersAsync(string userName, string firstname, string lastname);
        Task<ICollection<Diver>> GetAllRegisteredDiversAsync();
    }
}