using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Administration.SetUpRolesUseCase
{
    [UsedImplicitly]
    internal class SetUpRolesInteractor : IRequestHandler<SetUpRoles, UseCaseResult>
    {
        [NotNull] private readonly RoleManager<IdentityRole> roleManager;

        public SetUpRolesInteractor([NotNull] RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<UseCaseResult> Handle([NotNull] SetUpRoles request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            await roleManager.CreateAsync(new IdentityRole(Rolenames.Tauchbold));
            await roleManager.CreateAsync(new IdentityRole(Rolenames.Administrator));

            return UseCaseResult.Success();
        }
    }
}