using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Event;

namespace Tauchbolde.Application.Policies.Event.EventEdited
{
    [UsedImplicitly]
    internal class LogTelemetryEventEditedPolicy : INotificationHandler<EventEditedEvent>
    {
        [NotNull] private readonly ITelemetryService telemetryService;

        public LogTelemetryEventEditedPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }

#pragma warning disable 1998
        public async Task Handle([NotNull] EventEditedEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            
            telemetryService.TrackEvent(TelemetryEventNames.EditEvent, notification);
        }
#pragma warning restore 1998
    }
}