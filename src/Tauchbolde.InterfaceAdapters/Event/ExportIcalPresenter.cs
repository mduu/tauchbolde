using System.IO;
using Tauchbolde.Application.UseCases.Event.ExportIcalStreamUseCase;

namespace Tauchbolde.InterfaceAdapters.Event
{
    public class ExportIcalPresenter : IExportIcalStreamOutputPort
    {
        private Stream stream;
        
        public void Output(ExportIcalStreamOutput interactorOutput) => 
            stream = interactorOutput.IcalStream;

        public Stream GetIcalStream() => stream;
    }
}