using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Common.DomainServices.Repositories;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Logbook
{
    internal class LogbookService : ILogbookService
    {
        [NotNull] private readonly ILogbookEntryRepository logbookEntryRepository;

        public LogbookService(
            [NotNull] ILogbookEntryRepository logbookEntryRepository)
        {
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
        }
        
        public async Task<ICollection<LogbookEntry>> GetAllEntriesAsync() => 
            await logbookEntryRepository.GetAllEntriesAsync();
    }
}