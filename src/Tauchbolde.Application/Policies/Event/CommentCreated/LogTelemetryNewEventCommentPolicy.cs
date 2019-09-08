using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Event;

namespace Tauchbolde.Application.Policies.Event.CommentCreated
{
    [UsedImplicitly]
    internal class LogTelemetryNewEventCommentPolicy : INotificationHandler<CommentCreatedEvent>
    {
        private readonly ITelemetryService telemetryService;

        public LogTelemetryNewEventCommentPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }
        
#pragma warning disable 1998
        public async Task Handle([NotNull] CommentCreatedEvent notification, CancellationToken cancellationToken)
        {
            telemetryService.TrackEvent(TelemetryEventNames.NewEventComment, notification);
        }
#pragma warning restore 1998
    }
}