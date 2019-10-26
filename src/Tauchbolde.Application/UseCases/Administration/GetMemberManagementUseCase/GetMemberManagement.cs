using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Administration.GetMemberManagementUseCase
{
    public class GetMemberManagement : IRequest<UseCaseResult>
    {
        public GetMemberManagement(IMemberManagementOutputPort outputPort)
        {
            OutputPort = outputPort;
        }

        public IMemberManagementOutputPort OutputPort { get; }
    }
}