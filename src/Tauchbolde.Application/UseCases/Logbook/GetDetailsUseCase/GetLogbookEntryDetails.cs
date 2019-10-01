using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase
{
    public class GetLogbookEntryDetails : IRequest<UseCaseResult>
    {

        public GetLogbookEntryDetails(Guid logbookEntryId, [NotNull] ILogbookDetailOutputPort outputPort)
        {
            LogbookEntryId = logbookEntryId;
            OutputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }
        
        public Guid LogbookEntryId { get; }
        public ILogbookDetailOutputPort OutputPort { get; }
    }
}