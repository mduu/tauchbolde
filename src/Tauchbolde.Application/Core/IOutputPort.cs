namespace Tauchbolde.Application.Core
{
    public interface IOutputPort<in TInteractorOutput>
    {
        void Output(TInteractorOutput interactorOutput);
    }
}