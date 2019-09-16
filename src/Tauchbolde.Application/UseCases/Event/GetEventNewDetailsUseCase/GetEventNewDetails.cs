using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetEventNewDetailsUseCase
{
    public class GetEventNewDetails : IRequest<UseCaseResult>
    {
        public GetEventNewDetails([NotNull] string currentUserName, IGetEventNewDetailsOutputPort outputPort)
        {
            CurrentUserName = currentUserName ?? throw new ArgumentNullException(nameof(currentUserName));
            OutputPort = outputPort;
        }

        [NotNull] public string CurrentUserName { get; }
        [CanBeNull] public IGetEventNewDetailsOutputPort OutputPort { get; }
    }
}