using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.DataGateways
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<IGrouping<Diver, Notification>>> GetPendingNotificationByUserAsync();
    }
}
