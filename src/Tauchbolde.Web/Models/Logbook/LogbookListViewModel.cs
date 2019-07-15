using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Tauchbolde.Entities;

namespace Tauchbolde.Web.Models.Logbook
{
    public class LogbookListViewModel
    {
        public LogbookListViewModel([NotNull] ICollection<LogbookEntry> allEntries, bool allowEdit)
        {
            AllEntries = allEntries ?? throw new ArgumentNullException(nameof(allEntries));
            AllowEdit = allowEdit;
        }
        
        public ICollection<LogbookEntry> AllEntries { get; }
        public bool AllowEdit { get; }
    }
}