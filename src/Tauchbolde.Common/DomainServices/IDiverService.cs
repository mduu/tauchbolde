using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices
{
    public interface IDiverService
    {
        Task<ICollection<Diver>> GetMembersAsync(IDiverRepository diverRepository);
        Task<Diver> GetMemberAsync(IDiverRepository diverRepository, string userName);
    }
}