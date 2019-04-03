using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using Tauchbolde.Common.DomainServices;
using Xunit;
using Tauchbolde.Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApprovalTests.Namers;
using FakeItEasy;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.DomainServices.Notifications.HtmlFormatting;
using Tauchbolde.Common.DomainServices.TextFormatting;

namespace Tauchbolde.Tests.DomainServices.Notifications
{
    public class HtmlNotificationFormatterTests
    {
        private readonly HtmlFormatter formatter;

        public HtmlNotificationFormatterTests()
        {
            var urlGenerator1 = A.Fake<IUrlGenerator>();
            A.CallTo(() => urlGenerator1.GenerateEventUrl(A<Guid>._))
                .Returns("http://test/event/1");

            var notificationListFormatter = new HtmlListFormatter(
                urlGenerator1,
                new NotificationTypeInfos(),
                new MarkdownDigFormatter());

            formatter = new HtmlFormatter(
                new CssStyleFormatter(),
                new HtmlHeaderFormatter(),
                notificationListFormatter,
                new HtmlFooterFormatter());
        }

        [Theory]
        [InlineData(NotificationType.NewEvent)]
        [InlineData(NotificationType.CancelEvent)]
        [InlineData(NotificationType.EditEvent)]
        [InlineData(NotificationType.Commented)]
        [InlineData(NotificationType.Accepted)]
        [InlineData(NotificationType.Declined)]
        [InlineData(NotificationType.Neutral)]
        [InlineData(NotificationType.Tentative)]
        [UseReporter(typeof(DiffReporter))]
        public async Task Format(NotificationType notificationType)
        {
            using (ApprovalResults.ForScenario(notificationType.ToString()))
            {
                // Arrange
                var receiver = CreateDefaultReceiver();
                var notifications = CreateDefaultNotifications(notificationType);

                // Act
                var result = await formatter.FormatAsync(receiver, notifications);

                // Assert
                Approvals.VerifyHtml(result);
            }
        }

        private static Diver CreateDefaultReceiver() =>
            new Diver
            {
                Id = new Guid("4c3b714e-522f-4ef8-85f4-db74f0ccdd76"),
                Fullname = "John Doe",
                Firstname = "John",
                Lastname = "Doe",
            };

        private static IEnumerable<Notification> CreateDefaultNotifications(NotificationType notificationType) =>
            new List<Notification>()
            {
                new Notification
                {
                    Id = new Guid("fc1beb5a-5ad0-4deb-b35a-595d81d575e0"),
                    OccuredAt = new DateTime(2018, 10, 1, 14, 0, 0, DateTimeKind.Local),
                    Message = "Notification 1",
                    Type = notificationType,
                }
            };
    }
}