using System;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Profile.GetEditAvatarUseCase
{
    public class GetEditAvatar : IRequest<UseCaseResult>
    {
        public GetEditAvatar(Guid userId, IGetEditAvatarOutputPort outputPort)
        {
            UserId = userId;
            OutputPort = outputPort;
        }

        public Guid UserId { get; }
        public IGetEditAvatarOutputPort OutputPort { get; }
    }
}