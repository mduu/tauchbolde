using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using Tauchbolde.Common.DomainServices;
using Xunit;
using Tauchbolde.Common.Model;
using System.Collections.Generic;
using ApprovalTests.Namers;
using FakeItEasy;
using Tauchbolde.Common.DomainServices.Notifications;

namespace Tauchbolde.Tests.DomainServices.Notifications
{
    public class HtmlNotificationFormatterTests
    {
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
        public void Format(NotificationType notificationType)
        {
            using (ApprovalResults.ForScenario(notificationType.ToString()))
            {
                var urlGenerator = A.Fake<IUrlGenerator>();
                A.CallTo(() => urlGenerator.GenerateEventUrl(A<Guid>._)).Returns("http://test/event/1");
                var formatter = new HtmlNotificationFormatter(urlGenerator);
                var receiver = new Diver
                {
                    Id = new Guid("4c3b714e-522f-4ef8-85f4-db74f0ccdd76"),
                    Fullname = "John Doe",
                    Firstname = "John",
                    Lastname = "Doe",
                };
                var notifications = new List<Notification>()
                {
                    new Notification {
                        Id = new Guid("fc1beb5a-5ad0-4deb-b35a-595d81d575e0"),
                        OccuredAt = new DateTime(2018, 10, 1, 14, 0, 0),
                        Message = "Notification 1",
                        Type = notificationType,
                    }
                };

                var result = formatter.Format(receiver, notifications);

                Approvals.VerifyHtml(result);
            }
        }
    }
}
