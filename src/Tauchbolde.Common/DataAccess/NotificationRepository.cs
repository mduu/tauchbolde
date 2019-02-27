using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Common.DomainServices.Repositories;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DataAccess
{
    /// <summary>
    /// Repository for accessing <see cref="Notification"/> entities.
    /// </summary>
    internal class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IGrouping<Diver, Notification>>> GetPendingNotificationByUserAsync()
        {
            return await Context.Notifications
                .Include(n => n.Event)
                .Include(n => n.Recipient)
                    .ThenInclude(r => r.User)
                .Where(n => !n.AlreadySent)
                .GroupBy(n => n.Recipient)
                .ToListAsync();
        }
    }
}
