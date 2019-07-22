using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Tauchbolde.Domain;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Web.Models.ViewComponentModels
{
    public class LogbookSummaryListViewModel
    {
        public LogbookSummaryListViewModel([NotNull] IEnumerable<LogbookEntry> logbookEntries)
        {
            LogbookEntries = logbookEntries ?? throw new ArgumentNullException(nameof(logbookEntries));
        }

        public IEnumerable<LogbookEntry> LogbookEntries { get; }
    }
}