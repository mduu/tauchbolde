using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Tests.SharedKernel
{
    [UsedImplicitly]
    public class MyRequestHandler : IRequestHandler<MyRequest, UseCaseResult>
    {
#pragma warning disable 1998
        public async Task<UseCaseResult> Handle(MyRequest request, CancellationToken cancellationToken)
        {
            return UseCaseResult.Success();
        }
#pragma warning restore 1998
    }
}