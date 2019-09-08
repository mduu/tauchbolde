using System.IO;

namespace Tauchbolde.Application.UseCases.Event.ExportIcalStreamUseCase
{
    public class ExportIcalStreamOutput
    {
        public ExportIcalStreamOutput(Stream icalStream)
        {
            IcalStream = icalStream;
        }

        public Stream IcalStream { get; }
    }
}