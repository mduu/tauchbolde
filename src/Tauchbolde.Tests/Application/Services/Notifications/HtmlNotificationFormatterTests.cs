using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using FakeItEasy;
using Microsoft.Extensions.Options;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Tauchbolde.Driver.SmtpEmail;
using Tauchbolde.InterfaceAdapters;
using Tauchbolde.InterfaceAdapters.MailHtmlFormatting;
using Tauchbolde.InterfaceAdapters.TextFormatting;
using Xunit;

namespace Tauchbolde.Tests.Application.Services.Notifications
{
    public class HtmlNotificationFormatterTests
    {
        private readonly HtmlFormatter formatter;

        public HtmlNotificationFormatterTests()
        {
            var urlGenerator1 = A.Fake<IAbsoluteUrlGenerator>();
            A.CallTo(() => urlGenerator1.GenerateEventUrl(A<string>._, A<Guid>._))
                .ReturnsLazily(call => $"{call.Arguments[0]}event/1");
            
            IOptions<SmtpSenderConfiguration> smtpSenderOptions = 
                new OptionsWrapper<SmtpSenderConfiguration>(
                    new SmtpSenderConfiguration
                    {
                        RootUrl = "http://test/"
                    }
                );
            
            var notificationListFormatter = new HtmlListFormatter(
                urlGenerator1,
                new NotificationTypeInfos(),
                new MarkdownDigFormatter(),
                smtpSenderOptions);

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
        [InlineData(NotificationType.NewLogbookEntry)]
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
                    EventId = new Guid("8E83A186-7ACC-4DFC-9D75-6516EB623D72")
                }
            };
    }
}