using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Event.GetEventNewDetailsUseCase;

namespace Tauchbolde.InterfaceAdapters.Event.EditDetails
{
    [UsedImplicitly]
    public class MvcEventNewDetailsPresenter : IGetEventNewDetailsOutputPort
    {
        public void Output(GetEventNewOutput interactorOutput)
        {
            throw new System.NotImplementedException();
        }
    }
}