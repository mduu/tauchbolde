using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Web.Models.Logbook
{
    internal class LogbookListViewModel
    {
        public LogbookListViewModel([NotNull] ICollection<LogbookEntry> allEntries, bool allowEdit)
        {
            AllEntries = allEntries ?? throw new ArgumentNullException(nameof(allEntries));
            AllowEdit = allowEdit;
        }
        
        internal ICollection<LogbookEntry> AllEntries { get; }
        internal bool AllowEdit { get; }
    }
}