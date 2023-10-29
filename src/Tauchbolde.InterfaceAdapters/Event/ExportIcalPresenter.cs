using Tauchbolde.Application.UseCases.Event.ExportIcalStreamUseCase;

namespace Tauchbolde.InterfaceAdapters.Event
{
    public class ExportIcalPresenter : IExportIcalStreamOutputPort
    {
        private Stream stream;
        private string eventTitle;
        
        public void Output(ExportIcalStreamOutput interactorOutput)
        {
            stream = interactorOutput.IcalStream;
            eventTitle = interactorOutput.EventTitle;
        }

        public Stream GetIcalStream() => stream;
        public string GetDownloadFilename() => $"{eventTitle}.ics";
    }
}