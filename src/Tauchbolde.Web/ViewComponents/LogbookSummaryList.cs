using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Application.UseCases.Logbook.SummaryListUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Web.Models.ViewComponentModels;

namespace Tauchbolde.Web.ViewComponents
{
    public class LogbookSummaryList : ViewComponent
    {
        [NotNull] private readonly IMediator mediator;

        public LogbookSummaryList([NotNull] IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allEntries = await mediator.Send(new SummaryListLogbookEntries());
            Debug.Assert(allEntries != null);
            
            var model = new LogbookSummaryListViewModel(
                allEntries.IsSuccessful && allEntries.Payload != null
                    ? allEntries.Payload
                    : Enumerable.Empty<LogbookEntry>());
            
            return View(model);
        }

    }
}