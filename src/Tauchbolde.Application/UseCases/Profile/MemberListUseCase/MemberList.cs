using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Profile.MemberListUseCase
{
    public class MemberList : IRequest<UseCaseResult>
    {
        public MemberList(IMemberListOutputPort outputPort)
        {
            OutputPort = outputPort;
        }

        public IMemberListOutputPort OutputPort { get; }
    }
}