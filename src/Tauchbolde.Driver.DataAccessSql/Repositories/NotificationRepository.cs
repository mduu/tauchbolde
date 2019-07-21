using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    /// <summary>
    /// Repository for accessing <see cref="Notification"/> entities.
    /// </summary>
    internal class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

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
