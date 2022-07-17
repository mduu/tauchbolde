using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Administration.EditRolesUseCase
{
    [UsedImplicitly]
    internal class EditRolesInteractor : IRequestHandler<EditRoles, UseCaseResult>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly RoleManager<IdentityRole> roleManager;
        [NotNull] private readonly UserManager<IdentityUser> userManager;

        public EditRolesInteractor(
            [NotNull] ILogger<EditRolesInteractor> logger,
            [NotNull] RoleManager<IdentityRole> roleManager,
            [NotNull] UserManager<IdentityUser> userManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<UseCaseResult> Handle([NotNull] EditRoles request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            logger.LogInformation("Edit roles for user [{Username}]", request.UserName);

            var user = await userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                logger.LogError("User with username [{Username}] not found for editing!", request.UserName);
                return UseCaseResult.NotFound();
            }
            
            var allRoles = roleManager.Roles.ToArray();

            foreach (var role in allRoles)
            {
                if (request.Roles.Contains(role.Name))
                {
                    if (!await userManager.IsInRoleAsync(user, role.Name))
                    {
                        await userManager.AddToRoleAsync(user, role.Name);
                    }
                }
                else
                {
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }
            }

            logger.LogInformation("User with username [{Username}] updated with roles [{Roles}]!", request.UserName, request.Roles);
            return UseCaseResult.Success();
        }
    }
}