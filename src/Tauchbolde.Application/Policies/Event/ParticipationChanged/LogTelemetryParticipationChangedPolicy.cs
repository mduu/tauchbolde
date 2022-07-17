using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Event;

namespace Tauchbolde.Application.Policies.Event.ParticipationChanged
{
    [UsedImplicitly]
    internal class LogTelemetryParticipationChangedPolicy : INotificationHandler<ParticipationChangedEvent>
    {
        private readonly ITelemetryService telemetryService;

        public LogTelemetryParticipationChangedPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }
        
#pragma warning disable 1998
        public async Task Handle([NotNull] ParticipationChangedEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            
            telemetryService.TrackEvent(TelemetryEventNames.ParticipationChanged, notification);
        }
#pragma warning restore 1998
    }
}