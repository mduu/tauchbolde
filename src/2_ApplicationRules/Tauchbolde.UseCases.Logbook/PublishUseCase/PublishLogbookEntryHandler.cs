using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.UseCases.Logbook.DataAccess;

namespace Tauchbolde.UseCases.Logbook.PublishUseCase
{
    [UsedImplicitly]
    internal class PublishLogbookEntryHandler : IRequestHandler<PublishLogbookEntry, bool>
    {
        private readonly ILogger<PublishLogbookEntryHandler> logger;
        private readonly ILogbookDataAccess dataAccess;
        
        public PublishLogbookEntryHandler(
            [NotNull] ILogger<PublishLogbookEntryHandler> logger,
            [NotNull] ILogbookDataAccess dataAccess)
        {
            this.dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<bool> Handle([NotNull] PublishLogbookEntry request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            logger.LogInformation("Publish Logbook-Entry {logbookEntryId}", request.LogbookEntryId);

            var logbookEntry = await dataAccess.GetLogbookEntryByIdAsync(request.LogbookEntryId);
            if (logbookEntry == null)
            {
                logger.LogError("Logbook-Entry {logbookEntryId} not found", request.LogbookEntryId);
                return false;
            }

            try
            {
                logbookEntry.Publish();
                await dataAccess.UpdateLogbookEntryAsync(logbookEntry);
                await dataAccess.SaveChangesAsync();

                logger.LogInformation("Logbook-Entry {logbookEntryId} published successfull.", request.LogbookEntryId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error publishing logbook entry {logbookEntryId} {logbookEntryTitle}", logbookEntry.Id, logbookEntry.Title);
                return false;
            }

            return true;
        }
    }
}