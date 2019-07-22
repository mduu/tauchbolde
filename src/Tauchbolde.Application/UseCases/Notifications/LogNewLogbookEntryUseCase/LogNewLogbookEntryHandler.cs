using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Tauchbolde.Application.UseCases.Notifications.LogNewLogbookEntryUseCase
{
    public class LogNewLogbookEntryHandler : IRequestHandler<LogNewLogbookEntry>
    {
        public async Task<Unit> Handle(LogNewLogbookEntry request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
            
            return Unit.Value;
        }
    }
}