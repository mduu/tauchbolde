using System;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Profile
{
    public class GetUserProfile : IRequest<UseCaseResult>
    {
        public GetUserProfile(Guid userId, IGetUserProfileOutputPort outputPort)
        {
            UserId = userId;
            OutputPort = outputPort;
        }

        public Guid UserId { get; }
        public IGetUserProfileOutputPort OutputPort { get; }
    }
}