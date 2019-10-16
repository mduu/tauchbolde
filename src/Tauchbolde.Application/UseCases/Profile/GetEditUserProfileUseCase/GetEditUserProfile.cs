using System;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Profile.GetEditUserProfileUseCase
{
    public class GetEditUserProfile : IRequest<UseCaseResult>
    {
        public GetEditUserProfile(Guid userId, IGetEditUserProfileOutputPort outputPort)
        {
            UserId = userId;
            OutputPort = outputPort;
        }

        public Guid UserId { get; }
        public IGetEditUserProfileOutputPort OutputPort { get; }
    }
}