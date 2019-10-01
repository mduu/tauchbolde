using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Administration
{
    public class GetMassMailDetails : IRequest<UseCaseResult>
    {
        public GetMassMailDetails([NotNull] IGetMassMailDetailsOutputPort outputPort)
        {
            OutputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

        public IGetMassMailDetailsOutputPort OutputPort { get; }
    }
}