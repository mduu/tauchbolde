using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.Policies.Event.CommentDeleted;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Event;
using Xunit;

namespace Tauchbolde.Tests.Application.Policies.Events
{
    public class LogTelemetryDeleteEventCommentPolicyTests
    {
        private readonly Guid validCommentId = new Guid("697F20CB-9283-4821-9F87-9D56BBF5CD7C");
        private readonly ITelemetryService telemetryService = A.Fake<ITelemetryService>();
        private readonly LogTelemetryDeleteEventCommentPolicy policy;

        public LogTelemetryDeleteEventCommentPolicyTests()
        {
            policy = new LogTelemetryDeleteEventCommentPolicy(telemetryService);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var notification = new CommentDeletedEvent(validCommentId);
            
            // Act
            await policy.Handle(notification, CancellationToken.None);
            
            // Assert
            A.CallTo(() => telemetryService.TrackEvent(TelemetryEventNames.DeleteEventComment, notification))
                .MustHaveHappenedOnceExactly();
        }
        
        [Fact]
#pragma warning disable 1998
        public async Task Handle_NullNotification_MustFail()
        {
            // Act
            Func<Task> act = async () => await policy.Handle(null, CancellationToken.None);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
#pragma warning restore 1998

    }
}