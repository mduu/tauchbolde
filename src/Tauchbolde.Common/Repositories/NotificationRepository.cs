using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Repositories
{
    /// <summary>
    /// Repository for accessing <see cref="Notification"/> entities.
    /// </summary>
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
