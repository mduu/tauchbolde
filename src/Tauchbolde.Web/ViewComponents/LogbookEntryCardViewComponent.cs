using Microsoft.AspNetCore.Mvc;
using Tauchbolde.InterfaceAdapters.Logbook.ListAll;

namespace Tauchbolde.Web.ViewComponents
{
    public class LogbookEntryCardViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(LogbookListViewModel.LogbookItemViewModel logbookItem)
            => View(logbookItem);
    }
}