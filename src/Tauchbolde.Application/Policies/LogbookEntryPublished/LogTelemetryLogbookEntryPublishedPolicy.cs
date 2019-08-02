using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.LogbookEntry;

namespace Tauchbolde.Application.Policies.LogbookEntryPublished
{
    [UsedImplicitly]
    public class LogTelemetryLogbookEntryPublishedPolicy : INotificationHandler<LogbookEntryPublishedEvent>
    {
        private readonly ITelemetryService telemetryService;

        public LogTelemetryLogbookEntryPublishedPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }
        
#pragma warning disable 1998
        public async Task Handle([NotNull] LogbookEntryPublishedEvent notification, CancellationToken cancellationToken)
        {
            telemetryService.TrackEvent(TelemetryEventNames.LogbookEntryPublished, notification);
        }
#pragma warning restore 1998
    }
}