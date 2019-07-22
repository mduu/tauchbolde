using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Tests.SharedKernel
{
    public class MyRequestHandler : IRequestHandler<MyRequest, UseCaseResult>
    {
        public async Task<UseCaseResult> Handle(MyRequest request, CancellationToken cancellationToken)
        {
            return UseCaseResult.Success();
        }
    }
}