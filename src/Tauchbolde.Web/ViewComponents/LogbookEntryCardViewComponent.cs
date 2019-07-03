using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.Model;
using Tauchbolde.Web.Models.ViewComponentModels;

namespace Tauchbolde.Web.ViewComponents
{
    public class LogbookEntryCardViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(LogbookEntry logbookEntry, bool allowEdit)
        {
            return View(new LogbookCardViewModel(logbookEntry));
        }
    }
}