using System;
using System.Threading.Tasks;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public class NotificationSender : INotificationSender
    {
        public async Task Send(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, INotificationFormatter notificationFormatter)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

            var pendingNotifications = await notificationRepository.GetPendingNotificationByUser();
            foreach (var pendingNotification in pendingNotifications)
            {
                var recipient = pendingNotification.Key;
                if (!recipient.AdditionalUserInfos.LastNotificationCheckAt.HasValue ||
                    recipient.AdditionalUserInfos.LastNotificationCheckAt.Value.AddHours(
                        recipient.AdditionalUserInfos.NotificationIntervalInHours) > DateTime.Now)
                {

                    var content = notificationFormatter.Format(recipient, pendingNotification);

                    recipient.AdditionalUserInfos.LastNotificationCheckAt = DateTime.Now;
                }
            }

            throw new NotImplementedException();
        }
    }
}