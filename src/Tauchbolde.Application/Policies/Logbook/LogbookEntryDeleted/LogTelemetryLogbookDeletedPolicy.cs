using System;
using System.Threading;
using System.Threading.Tasks;
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
        
        public async Task Handle(LogbookEntryDeletedEvent notification, CancellationToken cancellationToken)
        {
            telemetryService.TrackEvent(TelemetryEventNames.LogbookEntryDeleted, notification);
        }
    }
}