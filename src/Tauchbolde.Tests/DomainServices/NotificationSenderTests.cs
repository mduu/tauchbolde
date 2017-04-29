using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Xunit;

namespace Tauchbolde.Tests.DomainServices
{
    public class NotificationSenderTests
    {
        [Fact]
        public async Task No_Check_Without_Pending_Notifications()
        {
            // Arrange
            var notificationRepository = A.Fake<INotificationRepository>();
            var formatter = A.Fake<INotificationFormatter>();
            var submitter = A.Fake<INotificationSubmitter>(o => o.Wrapping(new ConsoleNotificationSubmitter()));
            var sender = new NotificationSender();

            // Act
            await sender.Send(notificationRepository, formatter, submitter);

            // Assert
            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => formatter.Format(A<ApplicationUser>._, A<IGrouping<ApplicationUser, Notification>>._)).MustNotHaveHappened();
            A.CallTo(() => submitter.SubmitAsync(A<ApplicationUser>._, A<string>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task No_Check_Interval_Not_Reached()
        {
            // Arrange
            var userHansMeier = CreateTestUser(DateTime.Now.AddMinutes(-1), 1);
            var notifications = CreateTestNotifications(userHansMeier);
            var notificationRepository = A.Fake<INotificationRepository>();
            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).Returns(Task.FromResult(notifications.GroupBy(n => n.Recipient)));
            var formatter = A.Fake<INotificationFormatter>();
            var submitter = A.Fake<INotificationSubmitter>(o => o.Wrapping(new ConsoleNotificationSubmitter()));
            var sender = new NotificationSender();

            // Act
            await sender.Send(notificationRepository, formatter, submitter);

            // Assert
            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => formatter.Format(A<ApplicationUser>._, A<IGrouping<ApplicationUser, Notification>>._)).MustNotHaveHappened();
            A.CallTo(() => submitter.SubmitAsync(A<ApplicationUser>._, A<string>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task No_Check_Interval_Reached()
        {
            // Arrange
            var userHansMeier = CreateTestUser(DateTime.Now.AddHours(-1).AddMinutes(-1), 1);
            var notifications = CreateTestNotifications(userHansMeier);
            var notificationRepository = A.Fake<INotificationRepository>();
            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).Returns(Task.FromResult(notifications.GroupBy(n => n.Recipient)));
            var formatter = A.Fake<INotificationFormatter>();
            A.CallTo(() => formatter.Format(A<ApplicationUser>._, A<IGrouping<ApplicationUser, Notification>>._)).Returns("Some content!");
            var submitter = A.Fake<INotificationSubmitter>(o => o.Wrapping(new ConsoleNotificationSubmitter()));
            var sender = new NotificationSender();

            // Act
            await sender.Send(notificationRepository, formatter, submitter);

            // Assert
            A.CallTo(() => notificationRepository.GetPendingNotificationByUserAsync()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => formatter.Format(A<ApplicationUser>._, A<IGrouping<ApplicationUser, Notification>>._)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => submitter.SubmitAsync(A<ApplicationUser>._, A<string>._)).MustHaveHappened(Repeated.Exactly.Once);

            notifications.First().CountOfTries.Should().Be(1);
        }

        private static ApplicationUser CreateTestUser(DateTime lastNotificationCheckAt, int notificationIntervalInHours)
        {
            var userHansMeier = new ApplicationUser
            {
                Id = "hans.meier@test.com",
                UserName = "hans.meier@test.com",
                AdditionalUserInfos = new UserInfo
                {
                    Id = Guid.NewGuid(),
                    Firstname = "Hans",
                    Lastname = "Meier",
                    LastNotificationCheckAt = lastNotificationCheckAt,
                    NotificationIntervalInHours = notificationIntervalInHours,
                },
            };
            return userHansMeier;
        }

        private static List<Notification> CreateTestNotifications(ApplicationUser userHansMeier)
        {
            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = Guid.NewGuid(),
                    Recipient = userHansMeier,
                    AlreadySent = false,
                    CountOfTries = 0,
                    Message = "This is a test notification!",
                    Type = NotificationType.NewEvent,
                    Event = new Event
                    {
                        Name = "Testevent",
                        Id = Guid.NewGuid(),
                    }
                },
            };
            return notifications;
        }
    }
}
