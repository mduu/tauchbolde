using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;

namespace Tauchbolde.Application.UseCases.Logbook.PublishUseCase
{
    [UsedImplicitly]
    internal class PublishLogbookEntryHandler : IRequestHandler<PublishLogbookEntry, bool>
    {
        private readonly ILogger<PublishLogbookEntryHandler> logger;
        private readonly ILogbookEntryRepository dataAccess;
        
        public PublishLogbookEntryHandler(
            [NotNull] ILogger<PublishLogbookEntryHandler> logger,
            [NotNull] ILogbookEntryRepository dataAccess)
        {
            this.dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<bool> Handle([NotNull] PublishLogbookEntry request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            logger.LogInformation("Publish Logbook-Entry {logbookEntryId}", request.LogbookEntryId);

            var logbookEntry = await dataAccess.FindByIdAsync(request.LogbookEntryId);
            if (logbookEntry == null)
            {
                logger.LogError("Logbook-Entry {logbookEntryId} not found", request.LogbookEntryId);
                return false;
            }

            try
            {
                logbookEntry.Publish();
                await dataAccess.UpdateAsync(logbookEntry);

                logger.LogInformation("Logbook-Entry {logbookEntryId} published successfull.", request.LogbookEntryId);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error publishing logbook entry {logbookEntryId} {logbookEntryTitle}", logbookEntry.Id, logbookEntry.Title);
                return false;
            }
        }
    }
}