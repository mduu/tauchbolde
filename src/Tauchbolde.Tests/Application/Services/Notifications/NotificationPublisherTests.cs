using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Xunit;

namespace Tauchbolde.Tests.Application.Services.Notifications
{
    public class NotificationPublisherTests
    {
        private readonly NotificationPublisher publisher;
        private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
        private readonly ILogbookEntryRepository logbookEntryRepository = A.Fake<ILogbookEntryRepository>();
        private readonly INotificationRepository notificationRepository = A.Fake<INotificationRepository>();
        private readonly string validMessage = "This is a message";

        private readonly Diver john = new Diver
        {
            Id = new Guid("DDD9893E-4FDE-403D-9A2A-8E5944FA69A7"),
            Fullname = "John Doe",
            User = new IdentityUser("john.doe"),
            SendOwnNoticiations = false,
        };

        private readonly Diver jane = new Diver
        {
            Id = new Guid("FA2E1D7A-276E-4C24-B51E-1FCAD86AA75A"),
            Fullname = "Jane Doe",
            User = new IdentityUser("jane.doe"),
            SendOwnNoticiations = true,
        };

        public NotificationPublisherTests()
        {
            publisher = new NotificationPublisher(eventRepository, logbookEntryRepository, notificationRepository);
        }

        [Fact]
        public async Task PublishAsync_WithoutCurrentUser_MustSendToAll()
        {
            // Arrange
            var recipients = CreateValidRecipientList();
            
            // Act
            await publisher.PublishAsync(
                NotificationType.Accepted,
                validMessage,
                recipients,
                null,
                null,
                null);

            // Assert
            A.CallTo(() => notificationRepository.InsertAsync(A<Notification>._))
                .MustHaveHappenedTwiceExactly();
        }

        [Fact]
        public async Task PublishAsync_WithCurrentUserNotSendOwnNotification_MustSendToJaneOnly()
        {
            // Arrange
            var recipients = CreateValidRecipientList();
            
            // Act
            await publisher.PublishAsync(
                NotificationType.Accepted,
                validMessage,
                recipients,
                john,
                null,
                null);

            // Assert
            A.CallTo(() => notificationRepository.InsertAsync(A<Notification>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PublishAsync_WithCurrentUserSendOwnNotification_MustSendToJAll()
        {
            // Arrange
            var recipients = CreateValidRecipientList();
            
            // Act
            await publisher.PublishAsync(
                NotificationType.Accepted,
                validMessage,
                recipients,
                jane,
                null,
                null);

            // Assert
            A.CallTo(() => notificationRepository.InsertAsync(A<Notification>._))
                .MustHaveHappenedTwiceExactly();
        }

        private IEnumerable<Diver> CreateValidRecipientList() => new List<Diver> { john, jane };
    }
}