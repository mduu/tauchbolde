using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.Domain.Logbook;
using Tauchbolde.Common.Model;
using Tauchbolde.Web.Models.ViewComponentModels;

namespace Tauchbolde.Web.ViewComponents
{
    public class LogbookSummaryList : ViewComponent
    {
        [NotNull] private readonly ILogbookService logbookService;
        
        public LogbookSummaryList([NotNull] ILogbookService logbookService)
        {
            this.logbookService = logbookService ?? throw new ArgumentNullException(nameof(logbookService));
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allEntries = await logbookService.GetAllEntriesAsync() ?? Enumerable.Empty<LogbookEntry>();
            var model = new LogbookSummaryListViewModel(allEntries);
            
            return View(model);
        }

    }
}