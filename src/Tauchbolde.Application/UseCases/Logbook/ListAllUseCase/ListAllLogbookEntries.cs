using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.ListAllUseCase
{
    public class ListAllLogbookEntries : IRequest<UseCaseResult>
    {
        public ListAllLogbookEntries(
            [NotNull] IListLogbookEntriesOutputPort outputPort)
        {
            OutputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

        [NotNull] public IListLogbookEntriesOutputPort OutputPort { get; }
    }
}