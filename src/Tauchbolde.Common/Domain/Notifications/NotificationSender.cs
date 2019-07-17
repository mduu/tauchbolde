using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Entities;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.Domain.Notifications
{
    internal class NotificationSender : INotificationSender
    {
        private readonly ILogger logger;
        private readonly INotificationRepository notificationRepository;

        public NotificationSender(
            ILoggerFactory loggerFactory,
            [NotNull] INotificationRepository notificationRepository)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            logger = loggerFactory.CreateLogger<NotificationSender>();
            this.notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
        }

        /// <inheritdoc />
        public async Task SendAsync(
            INotificationFormatter notificationFormatter,
            INotificationSubmitter notificationSubmitter,
            Func<Task> saver)
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
                            var content = await notificationFormatter.FormatAsync(recipient, pendingNotificationsForRecipient);
                            if (!string.IsNullOrWhiteSpace(content))
                            {
                                await SubmitToRecipient(
                                    notificationSubmitter,
                                    pendingNotificationsForRecipient,
                                    recipient,
                                    content);
                            }

                            await UpdateDatabaseForRecipient(pendingNotificationsForRecipient, recipient, saver);
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

        private async Task UpdateDatabaseForRecipient(
            IGrouping<Diver, Notification> pendingNotificationsForRecipient,
            Diver recipient, 
            Func<Task> saver)
        {
            if (pendingNotificationsForRecipient == null) { throw new ArgumentNullException(nameof(pendingNotificationsForRecipient)); }
            if (recipient == null) { throw new ArgumentNullException(nameof(recipient)); }

            try
            {
                logger.LogTrace($"Updating database records for {pendingNotificationsForRecipient.Key} ...");

                recipient.LastNotificationCheckAt = DateTime.Now;
                await saver();

                logger.LogTrace($"Database updated for {pendingNotificationsForRecipient.Key}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error updating database for {pendingNotificationsForRecipient.Key}");
            }
        }
    }
}