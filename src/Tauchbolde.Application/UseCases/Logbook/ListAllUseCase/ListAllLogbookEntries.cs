using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.ListAllUseCase
{
    public class ListAllLogbookEntries : IRequest<UseCaseResult>
    {
        public ListAllLogbookEntries(bool includeUnpublished, [NotNull] IListLogbookEntriesPresenter presenter)
        {
            IncludeUnpublished = includeUnpublished;
            Presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        public bool IncludeUnpublished { get; }
        [NotNull] public IListLogbookEntriesPresenter Presenter { get; }
    }
}