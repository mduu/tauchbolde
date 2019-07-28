using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
using Tauchbolde.Driver.DataAccessSql.Mappers;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    /// <summary>
    /// Repository for accessing <see cref="NotificationData"/> entities.
    /// </summary>
    internal class NotificationRepository : RepositoryBase<Notification, NotificationData>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<IGrouping<Diver, Notification>>> GetPendingNotificationByUserAsync()
        {
            return
                (await Context.Notifications
                    .Include(n => n.Event)
                    .Include(n => n.Recipient)
                    .ThenInclude(r => r.User)
                    .Where(n => !n.AlreadySent)
                    .ToListAsync())
                .Select(n => n.MapTo())
                .GroupBy(n => n.Recipient)
                .ToList();
        }
               
        protected override Notification MapTo(NotificationData dataEntity) => dataEntity.MapTo();
        protected override NotificationData MapTo(Notification domainEntity) => domainEntity.MapTo();
    }
}