using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Events.LogbookEntry;

namespace Tauchbolde.Application.Policies.LogbookEntryUnpublished
{
    [UsedImplicitly]
    public class LogTelemetryLogbookEntryUnpublishedPolicy : INotificationHandler<LogbookEntryUnpublishedEvent>
    {
        private readonly ITelemetryService telemetryService;

        public LogTelemetryLogbookEntryUnpublishedPolicy([NotNull] ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }

#pragma warning disable 1998
        public async Task Handle(LogbookEntryUnpublishedEvent notification, CancellationToken cancellationToken)
        {
            telemetryService.TrackEvent(TelemetryEventNames.LogbookEntryUnpublished, notification);
        }
#pragma warning restore 1998
    }
}