using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase
{
    public class GetLogbookEntryDetails : IRequest<UseCaseResult>
    {

        public GetLogbookEntryDetails(Guid logbookEntryId, [NotNull] ILogbookDetailPresenter presenter, bool allowEdit)
        {
            LogbookEntryId = logbookEntryId;
            Presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            AllowEdit = allowEdit;
        }
        
        public Guid LogbookEntryId { get; }
        public ILogbookDetailPresenter Presenter { get; }
        public bool AllowEdit { get; }
    }
}