using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel.Services;

namespace Tauchbolde.Application.Services.Notifications
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
                    if (recipient.LastNotificationCheckAt.HasValue && recipient.LastNotificationCheckAt.Value.AddHours(
                        recipient.NotificationIntervalInHours) >= SystemClock.Now)
                    {
                        continue;
                    }

                    using (logger.BeginScope($"Send notification to {pendingNotificationsForRecipient.Key}"))
                    {
                        logger.LogInformation("Format email for recipient {Recipient}", pendingNotificationsForRecipient.Key);
                        
                        var content = await notificationFormatter.FormatAsync(recipient, pendingNotificationsForRecipient);
                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            logger.LogInformation("Sending to recipient {Recipient}", pendingNotificationsForRecipient.Key);
                            
                            await SubmitToRecipient(
                                notificationSubmitter,
                                pendingNotificationsForRecipient,
                                recipient,
                                content);
                            
                            logger.LogInformation("Sent to recipient {Recipient}", pendingNotificationsForRecipient.Key);
                        }
                        else
                        {
                            logger.LogInformation("Nothing to send for recipient {Recipient}", pendingNotificationsForRecipient.Key);
                        }

                        await UpdateDatabaseForRecipient(pendingNotificationsForRecipient, recipient, saver);
                        logger.LogInformation("Database updated for recipient {Recipient}", pendingNotificationsForRecipient.Key);
                    }
                }
            }
        }

        private async Task SubmitToRecipient(
            INotificationSubmitter notificationSubmitter,
            IGrouping<Diver, Notification> pendingNotificationsForRecipient,
            Diver recipient,
            string content)
        {
            if (notificationSubmitter == null)
            {
                throw new ArgumentNullException(nameof(notificationSubmitter));
            }

            try
            {
                await notificationSubmitter.SubmitAsync(recipient, content);
                foreach (var notification in pendingNotificationsForRecipient)
                {
                    notification.Sent();
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
                    notification.TriedSending();
                }
            }
        }

        private async Task UpdateDatabaseForRecipient(
            IGrouping<Diver, Notification> pendingNotificationsForRecipient,
            Diver recipient,
            Func<Task> saver)
        {
            if (pendingNotificationsForRecipient == null)
            {
                throw new ArgumentNullException(nameof(pendingNotificationsForRecipient));
            }

            if (recipient == null)
            {
                throw new ArgumentNullException(nameof(recipient));
            }

            try
            {
                logger.LogTrace($"Updating database records for {pendingNotificationsForRecipient.Key} ...");

                recipient.MarkNotificationChecked();
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