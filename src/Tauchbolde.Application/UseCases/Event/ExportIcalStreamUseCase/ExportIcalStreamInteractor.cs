using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.ExportIcalStreamUseCase
{
    [UsedImplicitly]
    public class ExportIcalStreamInteractor : IRequestHandler<ExportIcalStream, UseCaseResult>
    {
        [NotNull] private readonly IEventRepository repository;
        [NotNull] private readonly ITelemetryService telemetryService;

        public ExportIcalStreamInteractor(
            [NotNull] IEventRepository repository,
            [NotNull] ITelemetryService telemetryService)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.telemetryService = telemetryService;
        }

        public async Task<UseCaseResult> Handle([NotNull] ExportIcalStream request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var evt = await repository.FindByIdAsync(request.EventId);
            if (evt == null)
            {
                return UseCaseResult.NotFound();
            }

            request.OutputPort.Output(
                new ExportIcalStreamOutput(
                    new IcalBuilder()
                        .Id(evt.Id)
                        .TitlePrefix("🐠")
                        .Title(evt.Name)
                        .Description(evt.Description)
                        .Location(evt.Location)
                        .MeetingPoint(evt.MeetingPoint)
                        .CreateTime(request.CreateTime)
                        .StartTime(evt.StartTime)
                        .EndTime(evt.EndTime)
                        .Build(),
                    evt.Name));

            telemetryService.TrackEvent(TelemetryEventNames.IcalRequested, request);

            return UseCaseResult.Success();
        }
    }
}