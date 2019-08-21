using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Event;

namespace Tauchbolde.Application.Policies.Event.EventCommentEdited
{
    [UsedImplicitly]
    internal class LogTelemetryEditEventCommentPolicy : INotificationHandler<EditCommentEvent>
    {
        [NotNull] private readonly ITelemetryService telemetryService;

        public LogTelemetryEditEventCommentPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }

#pragma warning disable 1998
        public async Task Handle([NotNull] EditCommentEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            
            telemetryService.TrackEvent(TelemetryEventNames.EditEventComment, notification);
        }
#pragma warning restore 1998
    }
}