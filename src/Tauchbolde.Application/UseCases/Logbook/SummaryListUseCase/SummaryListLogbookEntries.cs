using System.Collections.Generic;
using MediatR;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.SummaryListUseCase
{
    public class SummaryListLogbookEntries : IRequest<UseCaseResult<IEnumerable<LogbookEntry>>>
    {
    }
}