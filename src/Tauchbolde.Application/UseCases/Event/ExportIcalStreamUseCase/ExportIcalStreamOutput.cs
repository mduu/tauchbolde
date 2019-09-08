using System;
using System.IO;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Event.ExportIcalStreamUseCase
{
    public class ExportIcalStreamOutput
    {
        public ExportIcalStreamOutput(Stream icalStream, [NotNull] string eventTitle)
        {
            IcalStream = icalStream;
            EventTitle = eventTitle ?? throw new ArgumentNullException(nameof(eventTitle));
        }

        public Stream IcalStream { get; }
        public string EventTitle { get; }
    }
}