using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.LogbookEntry;

namespace Tauchbolde.Application.Policies.Logbook.LogbookEntryEdited
{
    [UsedImplicitly]
    public class LogTelemetryLogbookEntryEditedPolicy : INotificationHandler<LogbookEntryEditedEvent>
    {
        private readonly ITelemetryService telemetryService;

        public LogTelemetryLogbookEntryEditedPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }

#pragma warning disable 1998
        public async Task Handle(LogbookEntryEditedEvent notification, CancellationToken cancellationToken)
        {
            telemetryService.TrackEvent(TelemetryEventNames.LogbookEntryEdited, notification);
        }
#pragma warning restore 1998
    }
}