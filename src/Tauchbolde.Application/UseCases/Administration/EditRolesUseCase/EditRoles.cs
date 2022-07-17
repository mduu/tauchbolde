using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Administration.EditRolesUseCase
{
    public class EditRoles : IRequest<UseCaseResult>
    {
        public EditRoles([NotNull] string userName, [NotNull] IEnumerable<string> roles)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Roles = roles ?? throw new ArgumentNullException(nameof(roles));
        }

        [NotNull] public string UserName { get; }
        [NotNull] public IEnumerable<string> Roles { get; }
    }
}