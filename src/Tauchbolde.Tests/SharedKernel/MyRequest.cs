using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Tests.SharedKernel
{
    public class MyRequest : IRequest<UseCaseResult>
    {
        public MyRequest(int number)
        {
            Number = number;
        }

        public int Number { get; }
    }
}