using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    internal class NotificationSender : INotificationSender
    {
        private readonly ILogger logger;
        private readonly ApplicationDbContext databaseContext;

        public NotificationSender(
            ILoggerFactory loggerFactory,
            ApplicationDbContext databaseContext)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            logger = loggerFactory.CreateLogger<NotificationSender>();
            this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        }

        public async Task SendAsync(
            INotificationRepository notificationRepository,
            INotificationFormatter notificationFormatter,
            INotificationSubmitter notificationSubmitter)
        {
            if (notificationRepository == null) throw new ArgumentNullException(nameof(notificationRepository));

            using (logger.BeginScope("SendAsync"))
            {
                var pendingNotifications = await notificationRepository.GetPendingNotificationByUserAsync();
                foreach (var pendingNotificationsForRecipient in pendingNotifications)
                {
                    var recipient = pendingNotificationsForRecipient.Key;
                    if (!recipient.LastNotificationCheckAt.HasValue ||
                        recipient.LastNotificationCheckAt.Value.AddHours(
                            recipient.NotificationIntervalInHours) < DateTime.Now)
                    {
                        using (logger.BeginScope($"Send notification to {pendingNotificationsForRecipient.Key}"))
                        {
                            var content = notificationFormatter.Format(recipient, pendingNotificationsForRecipient);
                            if (!string.IsNullOrWhiteSpace(content))
                            {
                                await SubmitToRecipient(
                                    notificationSubmitter,
                                    pendingNotificationsForRecipient,
                                    recipient,
                                    content);
                            }

                            await UpdateDatabaseForRecipient(pendingNotificationsForRecipient, recipient);
                        }
                    }
                }
            }
        }

        private async Task SubmitToRecipient(
            INotificationSubmitter notificationSubmitter,
            System.Linq.IGrouping<Diver, Notification> pendingNotificationsForRecipient,
            Diver recipient,
            string content)
        {
            if (notificationSubmitter == null) { throw new ArgumentNullException(nameof(notificationSubmitter)); }

            try
            {
                await notificationSubmitter.SubmitAsync(recipient, content);
                foreach (var notification in pendingNotificationsForRecipient)
                {
                    notification.AlreadySent = true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error submitting notification to {recipient.Fullname}");
            }
            finally
            {
                foreach (var notification in pendingNotificationsForRecipient)
                {
                    notification.CountOfTries++;
                }
            }
        }

        private async Task UpdateDatabaseForRecipient(System.Linq.IGrouping<Diver, Notification> pendingNotificationsForRecipient, Diver recipient)
        {
            if (pendingNotificationsForRecipient == null) { throw new ArgumentNullException(nameof(pendingNotificationsForRecipient)); }
            if (recipient == null) { throw new ArgumentNullException(nameof(recipient)); }

            try
            {
                logger.LogTrace($"Updating database records for {pendingNotificationsForRecipient.Key} ...");

                recipient.LastNotificationCheckAt = DateTime.Now;
                await databaseContext.SaveChangesAsync();

                logger.LogTrace($"Database updated for {pendingNotificationsForRecipient.Key}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error updating database for {pendingNotificationsForRecipient.Key}");
            }
        }
    }
}