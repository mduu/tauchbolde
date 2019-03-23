using System;
using JetBrains.Annotations;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Web.Models.ViewComponentModels
{
    public class LogbookCardViewModel
    {
        public LogbookCardViewModel([NotNull] LogbookEntry logbookEntry, bool allowEdit)
        {
            LogbookEntry = logbookEntry ?? throw new ArgumentNullException(nameof(logbookEntry));
            AllowEdit = allowEdit;
        }
        
        public LogbookEntry LogbookEntry { get; }
        public bool AllowEdit { get; }
    }
}