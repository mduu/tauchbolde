using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.Policies.Profile.AvatarChanged;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Diver;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.Policies.Profile
{
    public class LogTelemetryAvatarChangedPolicyTests
    {
        private readonly LogTelemetryAvatarChangedPolicy policy;
        private readonly ITelemetryService telemetryService = A.Fake<ITelemetryService>();

        public LogTelemetryAvatarChangedPolicyTests()
        {
            policy = new LogTelemetryAvatarChangedPolicy(telemetryService);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var notification = new UserProfileEditedEvent(
                DiverFactory.JohnDoeDiverId, 
                DiverFactory.JaneDoeDiverId);
            
            // Act
            await policy.Handle(notification, CancellationToken.None);

            // Assert
            A.CallTo(() => telemetryService.TrackEvent(TelemetryEventNames.AvatarChanged, A<object>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Handle_NullNotification_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var act = () => policy.Handle(null, CancellationToken.None);
            
            // Assert
            act.Should().ThrowAsync<ArgumentNullException>().Result.Which.ParamName.Should().Be("notification");
        }
    }
}