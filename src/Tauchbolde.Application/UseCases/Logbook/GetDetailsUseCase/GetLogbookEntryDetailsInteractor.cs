using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase
{
    [UsedImplicitly]
    internal class GetLogbookEntryDetailsInteractor : IRequestHandler<GetLogbookEntryDetails, UseCaseResult>
    {
        [NotNull] private readonly ILogger<GetLogbookEntryDetailsInteractor> logger;
        [NotNull] private readonly ILogbookEntryRepository repository;
        [NotNull] private readonly ICurrentUser currentUser;

        public GetLogbookEntryDetailsInteractor(
            [NotNull] ILogger<GetLogbookEntryDetailsInteractor> logger,
            [NotNull] ILogbookEntryRepository repository,
            [NotNull] ICurrentUser currentUser)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<UseCaseResult> Handle([NotNull] GetLogbookEntryDetails request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            logger.LogDebug("Get details for LogbookEntry [{id}]", request.LogbookEntryId);

            var details = await repository.FindByIdAsync(request.LogbookEntryId);
            if (details == null)
            {
                return UseCaseResult<LogbookEntry>.NotFound(
                    nameof(LogbookEntry),
                    nameof(GetLogbookEntryDetails.LogbookEntryId),
                    request.LogbookEntryId);
            }

            var allowEdit = await currentUser.GetIsTauchboldOrAdminAsync();
            
            var output = new GetLogbookEntryDetailOutput(
                details.Id,
                allowEdit,
                details.Title,
                details.TeaserText,
                details.Text,
                details.ExternalPhotoAlbumUrl,
                details.TeaserImage,
                details.TeaserImageThumb,
                details.Event?.Name,
                details.EventId,
                details.IsFavorite,
                details.IsPublished,
                details.OriginalAuthor.Fullname,
                details.OriginalAuthor.User?.Email,
                details.OriginalAuthor.AvatarId,
                details.EditorAuthor?.Fullname,
                details.EditorAuthor?.User?.Email,
                details.EditorAuthor?.AvatarId,
                details.CreatedAt,
                details.ModifiedAt);

            request.OutputPort.Output(output);

            logger.LogDebug("Got details for LogbookEntry [{id}] successful", request.LogbookEntryId);

            return UseCaseResult.Success();
        }
    }
}