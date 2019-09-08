using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.ExportIcalStreamUseCase
{
    public class ExportIcalStream : IRequest<UseCaseResult>
    {
        public ExportIcalStream(Guid eventId, [NotNull] IExportIcalStreamOutputPort outputPort, DateTime? createTime = null)
        {
            EventId = eventId;
            OutputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
            CreateTime = createTime;
        }

        public Guid EventId { get; }
        public DateTime? CreateTime { get; }
        public IExportIcalStreamOutputPort OutputPort { get; }
        
    }
}