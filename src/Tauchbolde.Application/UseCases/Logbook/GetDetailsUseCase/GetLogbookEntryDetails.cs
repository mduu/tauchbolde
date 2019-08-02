using System;
using MediatR;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase
{
    public class GetLogbookEntryDetails : IRequest<UseCaseResult<LogbookEntry>>
    {

        public GetLogbookEntryDetails(Guid logbookEntryId)
        {
            LogbookEntryId = logbookEntryId;
        }
        
        public Guid LogbookEntryId { get; }
    }
}