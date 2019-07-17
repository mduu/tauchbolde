using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.UseCases.Logbook.DataAccess;

namespace Tauchbolde.UseCases.Logbook.UnpublishUseCase
{
    [UsedImplicitly]
    internal class UnpublishLogbookEntryHandler : IRequestHandler<UnpublishLogbookEntry, bool>
    {
        private readonly ILogger<UnpublishLogbookEntryHandler> logger;
        private readonly ILogbookDataAccess dataAccess;
        
        public UnpublishLogbookEntryHandler(
            [NotNull] ILogger<UnpublishLogbookEntryHandler> logger,
            [NotNull] ILogbookDataAccess dataAccess)
        {
            this.dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<bool> Handle([NotNull] UnpublishLogbookEntry request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            logger.LogInformation("Unpublish Logbook-Entry {logbookEntryId}", request.LogbookEntryId);

            var logbookEntry = await dataAccess.GetLogbookEntryByIdAsync(request.LogbookEntryId);
            if (logbookEntry == null)
            {
                logger.LogError("Logbook-Entry {logbookEntryId} not found", request.LogbookEntryId);
                return false;
            }

            try
            {
                logbookEntry.Unpublish();
                await dataAccess.UpdateLogbookEntryAsync(logbookEntry);
                await dataAccess.SaveChangesAsync();

                logger.LogInformation("Logbook-Entry {logbookEntryId} unpublished successfull.", request.LogbookEntryId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error unpublishing logbook entry {logbookEntryId} {logbookEntryTitle}", logbookEntry.Id, logbookEntry.Title);
            }

            return true;
        }
    }
}