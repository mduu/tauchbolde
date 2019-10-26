using Microsoft.AspNetCore.Mvc;
using Tauchbolde.InterfaceAdapters.MVC.Presenters.Logbook.ListAll;

namespace Tauchbolde.Web.ViewComponents
{
    public class LogbookEntryCardViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(MvcLogbookListViewModel.LogbookItemViewModel logbookItem)
            => View(logbookItem);
    }
}