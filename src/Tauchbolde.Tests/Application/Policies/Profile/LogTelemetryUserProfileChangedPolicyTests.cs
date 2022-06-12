using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using Tauchbolde.Application.Policies.Profile.UserProfileEdited;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Diver;
using Xunit;

namespace Tauchbolde.Tests.Application.Policies.Profile
{
    public class LogTelemetryUserProfileChangedPolicyTests
    {
        [NotNull] private readonly LogTelemetryUserProfileChangedPolicy policy;
        [NotNull] private readonly ITelemetryService telemetryService = A.Fake<ITelemetryService>();

        public LogTelemetryUserProfileChangedPolicyTests()
        {
            policy = new LogTelemetryUserProfileChangedPolicy(telemetryService);
        }

        [Fact]
        public async Task Handle_Success_MustTriggerTelemetry()
        {
            // Arrange
            var notification = new UserProfileEditedEvent(
                new Guid("45DA87DA-1EE6-4508-8CE6-061304FF975F"),
                new Guid("166DCAD2-0ABD-4E22-B4FA-D7A6F92B4C76"));
        
            // Act
            await policy.Handle(notification, CancellationToken.None);

            // Assert
            A.CallTo(() => telemetryService.TrackEvent(TelemetryEventNames.UserProfileEdited, A<object>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Handle_NullNotification_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var act = () => policy.Handle(null, CancellationToken.None);
            
            // Arrange
            act.Should().ThrowAsync<ArgumentNullException>().Result.Which.ParamName.Should().Be("notification");
        }
    }
}