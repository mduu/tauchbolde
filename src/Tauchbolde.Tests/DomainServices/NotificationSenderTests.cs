using System.Threading.Tasks;
using FakeItEasy;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.Repositories;
using Xunit;

namespace Tauchbolde.Tests.DomainServices
{
    public class NotificationSenderTests
    {
        [Fact]
        public void No_Check_When_Not_In_Interval()
        {
            // Arrange
            var notificationRepository = A.Fake<INotificationRepository>();
            var userRepository = A.Fake<IApplicationUserRepository>();
            var formatter = A.Fake<INotificationFormatter>();
            var submitter = A.Fake<INotificationSubmitter>(o => o.Wrapping(new ConsoleNotificationSubmitter()));

            var sender = new NotificationSender();

            // Act
            sender.Send(notificationRepository, userRepository, formatter, submitter).ContinueWith((o) => { });

            // Assert
        }
    }
}
