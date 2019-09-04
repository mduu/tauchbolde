using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.ListAllUseCase
{
    public class ListAllLogbookEntries : IRequest<UseCaseResult>
    {
        public ListAllLogbookEntries(bool includeUnpublished, [NotNull] IListLogbookEntriesOutputPort outputPort)
        {
            IncludeUnpublished = includeUnpublished;
            OutputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

        public bool IncludeUnpublished { get; }
        [NotNull] public IListLogbookEntriesOutputPort OutputPort { get; }
    }
}