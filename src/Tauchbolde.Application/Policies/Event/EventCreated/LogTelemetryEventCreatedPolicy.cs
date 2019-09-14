using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Event;

namespace Tauchbolde.Application.Policies.Event.EventCreated
{
    [UsedImplicitly]
    internal class LogTelemetryEventCreatedPolicy : INotificationHandler<EventCreatedEvent>
    {
        [NotNull] private readonly ITelemetryService telemetryService;

        public LogTelemetryEventCreatedPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }

#pragma warning disable 1998
        public async Task Handle([NotNull] EventCreatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            
            telemetryService.TrackEvent(TelemetryEventNames.NewEvent, notification);
        }
#pragma warning restore 1998
    }
}