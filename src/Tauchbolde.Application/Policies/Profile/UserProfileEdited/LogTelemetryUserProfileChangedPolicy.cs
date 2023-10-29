using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.Diver;

namespace Tauchbolde.Application.Policies.Profile.UserProfileEdited
{
    [UsedImplicitly]
    internal class LogTelemetryUserProfileChangedPolicy : INotificationHandler<UserProfileEditedEvent>
    {
        [NotNull] private readonly ITelemetryService telemetryService;

        public LogTelemetryUserProfileChangedPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }
        
#pragma warning disable 1998
        public async Task Handle([NotNull] UserProfileEditedEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            
            telemetryService.TrackEvent(TelemetryEventNames.UserProfileEdited, notification);
        }
#pragma warning restore 1998
    }
}