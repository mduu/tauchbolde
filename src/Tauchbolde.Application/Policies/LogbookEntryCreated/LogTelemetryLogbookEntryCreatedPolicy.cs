using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.LogbookEntry;

namespace Tauchbolde.Application.Policies.LogbookEntryCreated
{
    [UsedImplicitly]
    public class LogTelemetryLogbookEntryCreatedPolicy : INotificationHandler<LogbookEntryCreatedEvent>
    {
        private readonly ITelemetryService telemetryService;

        public LogTelemetryLogbookEntryCreatedPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }
        
#pragma warning disable 1998
        public async Task Handle(LogbookEntryCreatedEvent notification, CancellationToken cancellationToken)
        {
            telemetryService.TrackEvent(TelemetryEventNames.LogbookEntryCreated, notification);
        }
#pragma warning restore 1998
    }
}