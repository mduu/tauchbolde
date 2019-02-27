using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Repositories
{
    /// <summary>
    /// Data-access for <see cref="Notification"/> entities.
    /// </summary>
    public interface INotificationRepository : IRepository<Notification>
    {
        /// <summary>
        /// Gets all pending notification grouped by user asynchronous.
        /// </summary>
        /// <returns>All pending notification grouped by user.</returns>
        Task<IEnumerable<IGrouping<Diver, Notification>>> GetPendingNotificationByUserAsync();
    }
}
