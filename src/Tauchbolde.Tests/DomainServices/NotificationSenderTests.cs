using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
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
            var userRepository = A.Fake<IApplicationUserRepository>();
            var formatter = A.Fake<INotificationFormatter>();
            var submitter = A.Fake<INotificationSubmitter>(o => o.Wrapping(new ConsoleNotificationSubmitter()));

            var sender = new NotificationSender();

            // Act
            await sender.Send(
                notificationRepository,
                userRepository,
                formatter,
                submitter);

            // Assert
            A.CallTo(() => notificationRepository.GetPendingNotificationByUser()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => formatter.Format(A<ApplicationUser>._, A<IGrouping<ApplicationUser, Notification>>._)).MustNotHaveHappened();
            A.CallTo(() => submitter.SubmitAsync(A<ApplicationUser>._, A<string>._)).MustNotHaveHappened();
        }
    }
}
