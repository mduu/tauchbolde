using System.Collections.Generic;
using MediatR;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.ListAllUseCase
{
    public class ListAllLogbookEntries : IRequest<UseCaseResult<IEnumerable<LogbookEntry>>>
    {
        public ListAllLogbookEntries(bool includeUnpublished)
        {
            IncludeUnpublished = includeUnpublished;
        }

        public bool IncludeUnpublished { get; }
    }
}