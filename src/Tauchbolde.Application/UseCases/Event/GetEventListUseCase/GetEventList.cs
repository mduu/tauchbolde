using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Event.GetEventListUseCase
{
    public class GetEventList : IRequest<UseCaseResult>
    {
        public GetEventList([NotNull] IEventListOutputPort outputPort)
        {
            OutputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

        [NotNull] public IEventListOutputPort OutputPort { get; }
    }
}