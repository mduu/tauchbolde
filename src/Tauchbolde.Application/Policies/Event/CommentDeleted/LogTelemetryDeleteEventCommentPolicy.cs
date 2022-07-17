using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Event;

namespace Tauchbolde.Application.Policies.Event.CommentDeleted
{
    [UsedImplicitly]
    public class LogTelemetryDeleteEventCommentPolicy : INotificationHandler<CommentDeletedEvent>
    {
        private readonly ITelemetryService telemetryService;

        public LogTelemetryDeleteEventCommentPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }
        
#pragma warning disable 1998
        public async Task Handle([NotNull] CommentDeletedEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            
            telemetryService.TrackEvent(TelemetryEventNames.DeleteEventComment, notification);
        }
#pragma warning restore 1998
    }
}