using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Administration.GetEditRolesUseCase
{
    [UsedImplicitly]
    internal class GetEditRolesInteractor : IRequestHandler<GetEditRoles, UseCaseResult>
    {
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly UserManager<IdentityUser> userManager;
        [NotNull] private readonly RoleManager<IdentityRole> roleManager;
        [NotNull] private readonly ICurrentUser currentUser;

        public GetEditRolesInteractor(
            [NotNull] IDiverRepository diverRepository,
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] RoleManager<IdentityRole> roleManager,
            [NotNull] ICurrentUser currentUser)
        {
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<UseCaseResult> Handle([NotNull] GetEditRoles request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            var diver = await diverRepository.FindByUserNameAsync(request.UserName);
            if (diver == null)
            {
                return UseCaseResult.NotFound();
            }

            var allRoles = roleManager.Roles.Select(r => r.Name);
            var assignedRoles = await userManager.GetRolesAsync(diver.User);
            
            request.OutputPort?.Output(
                new GetEditRolesOutput(
                    diver.User.UserName,
                    diver.Fullname,
                    allRoles,
                    assignedRoles));

            return UseCaseResult.Success();
        }
    }
}