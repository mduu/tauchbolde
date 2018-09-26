using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repository;

namespace Tauchbolde.Common.Repositories
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
        Task<IEnumerable<IGrouping<UserInfo, Notification>>> GetPendingNotificationByUserAsync();
    }
}
