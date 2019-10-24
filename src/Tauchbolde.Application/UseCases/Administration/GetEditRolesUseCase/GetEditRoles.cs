using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Administration.GetEditRolesUseCase
{
    public class GetEditRoles : IRequest<UseCaseResult>
    {
        public GetEditRoles([NotNull] string userName, IGetEditRolesOutputPort outputPort)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            OutputPort = outputPort;
        }

        [NotNull] public string UserName { get; }
        public IGetEditRolesOutputPort OutputPort { get; }
    }
}