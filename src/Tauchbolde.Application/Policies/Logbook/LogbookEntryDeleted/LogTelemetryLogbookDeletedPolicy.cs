using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.LogbookEntry;

namespace Tauchbolde.Application.Policies.Logbook.LogbookEntryDeleted
{
    [UsedImplicitly]
    public class LogTelemetryLogbookDeletedPolicy : INotificationHandler<LogbookEntryDeletedEvent>
    {
        private readonly ITelemetryService telemetryService;

        public LogTelemetryLogbookDeletedPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }
        
#pragma warning disable 1998
        public async Task Handle(LogbookEntryDeletedEvent notification, CancellationToken cancellationToken)
        {
            telemetryService.TrackEvent(TelemetryEventNames.LogbookEntryDeleted, notification);
        }
#pragma warning restore 1998
    }
}