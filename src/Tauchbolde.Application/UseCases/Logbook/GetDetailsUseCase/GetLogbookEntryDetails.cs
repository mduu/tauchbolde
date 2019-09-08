using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase
{
    public class GetLogbookEntryDetails : IRequest<UseCaseResult>
    {

        public GetLogbookEntryDetails(Guid logbookEntryId, [NotNull] ILogbookDetailOutputPort outputPort, bool allowEdit)
        {
            LogbookEntryId = logbookEntryId;
            OutputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
            AllowEdit = allowEdit;
        }
        
        public Guid LogbookEntryId { get; }
        public ILogbookDetailOutputPort OutputPort { get; }
        public bool AllowEdit { get; }
    }
}