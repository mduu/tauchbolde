using System.Threading.Tasks;

namespace Tauchbolde.Application.Core
{
    public interface IPresenter<in TInteractorOutput>
    {
        Task PresentAsync(TInteractorOutput interactorOutput);
    }
}