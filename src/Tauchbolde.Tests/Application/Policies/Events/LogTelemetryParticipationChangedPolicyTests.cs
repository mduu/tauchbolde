using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.Policies.Event.ParticipationChanged;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;
using Xunit;

namespace Tauchbolde.Tests.Application.Policies.Events
{
    public class LogTelemetryParticipationChangedPolicyTests
    {
        private readonly ITelemetryService telemetryService = A.Fake<ITelemetryService>();
        private readonly LogTelemetryParticipationChangedPolicy policy;

        public LogTelemetryParticipationChangedPolicyTests()
        {
            policy = new LogTelemetryParticipationChangedPolicy(telemetryService);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var notification = CreateValidParticipationChangedEvent();

            // Act
            await policy.Handle(notification, CancellationToken.None);

            // Assert
            A.CallTo(() => telemetryService.TrackEvent(TelemetryEventNames.ParticipationChanged, A<object>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
#pragma warning disable 1998
        public async Task Handle_NullNotification_MustFail()
        {
            // Act
            var act = async () => await policy.Handle(null, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
#pragma warning restore 1998

        private static ParticipationChangedEvent CreateValidParticipationChangedEvent() =>
            new ParticipationChangedEvent(
                new Guid("03A25387-682A-4383-BCF2-D8B99802E10E"),
                new Guid("C47B17EA-0080-406B-8A21-947335AF4CB7"),
                new Guid("3F7DE587-D69C-41C7-92A3-CA9C33E43686"),
                ParticipantStatus.Accepted);
    }
}