using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.Policies.Event.EventCreated;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Event;
using Xunit;

namespace Tauchbolde.Tests.Application.Policies.Events
{
    public class LogTelemetryEventCreatedPolicyTests
    {
        private readonly LogTelemetryEventCreatedPolicy policy;
        private readonly ITelemetryService telemetryService = A.Fake<ITelemetryService>();
        private readonly Guid validEventId = new Guid("ABF7A045-FA17-47FD-A225-E4B2BC5C35DB");

        public LogTelemetryEventCreatedPolicyTests()
        {
            policy = new LogTelemetryEventCreatedPolicy(telemetryService);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var notification = new EventCreatedEvent(validEventId);
     
            // Act
            await policy.Handle(notification, CancellationToken.None);

            // Assert
            A.CallTo(() => telemetryService.TrackEvent(TelemetryEventNames.NewEvent, A<object>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Handle_NullNotification_MustFail()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var act = () => policy.Handle(null, CancellationToken.None);
            
            // Assert
            act.Should().ThrowAsync<ArgumentNullException>().Result.Which.ParamName.Should().Be("notification");
        }
    }
}