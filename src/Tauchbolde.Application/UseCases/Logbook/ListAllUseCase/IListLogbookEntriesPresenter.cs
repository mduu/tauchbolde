using System.Threading.Tasks;

namespace Tauchbolde.Application.UseCases.Logbook.ListAllUseCase
{
    public interface IListLogbookEntriesPresenter
    {
        Task PresentAsync(ListAllLogbookEntriesOutputPort output);
    }
}