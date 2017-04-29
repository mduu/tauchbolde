using System;
using System.Threading.Tasks;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    public class NotificationSender : INotificationSender
    {
        public async Task Send(
            INotificationRepository notificationRepository,
            INotificationFormatter notificationFormatter,
            INotificationSubmitter notificationSubmitter)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));

            // Check
            var pendingNotifications = await notificationRepository.GetPendingNotificationByUserAsync();
            foreach (var pendingNotification in pendingNotifications)
            {
                var recipient = pendingNotification.Key;
                if (!recipient.AdditionalUserInfos.LastNotificationCheckAt.HasValue ||
                    recipient.AdditionalUserInfos.LastNotificationCheckAt.Value.AddHours(
                        recipient.AdditionalUserInfos.NotificationIntervalInHours) < DateTime.Now)
                {
                    // Format
                    var content = notificationFormatter.Format(recipient, pendingNotification);
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        try
                        {
                            // Submit
                            await notificationSubmitter.SubmitAsync(recipient, content);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                        finally
                        {
                            foreach (var notification in pendingNotification)
                            {
                                notification.CountOfTries++;
                            }
                        }
                    }

                    recipient.AdditionalUserInfos.LastNotificationCheckAt = DateTime.Now;
                }
            }
        }
    }
}