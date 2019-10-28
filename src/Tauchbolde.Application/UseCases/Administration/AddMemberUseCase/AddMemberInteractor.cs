using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Administration.AddMemberUseCase
{
    [UsedImplicitly]
    internal class AddMemberInteractor : IRequestHandler<AddMember, UseCaseResult<string>>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly UserManager<IdentityUser> userManager;
        [NotNull] private readonly ICurrentUser currentUser;

        public AddMemberInteractor(
            [NotNull] ILogger<AddMemberInteractor> logger,
            [NotNull] IDiverRepository diverRepository,
            [NotNull] UserManager<IdentityUser> userManager, 
            [NotNull] ICurrentUser currentUser)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<UseCaseResult<string>> Handle([NotNull] AddMember request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            logger.LogInformation("Adding member [{userName}]", request.UserName, request);

            if (!await currentUser.GetIsAdminAsync())
            {
                logger.LogError("Access denied for user [{currentUser}]!", currentUser.Username);
                return UseCaseResult<string>.AccessDenied();
            }
            
            var user = await userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                logger.LogError("Identity user [{username}] not found!", request.UserName);
                return UseCaseResult<string>.NotFound();
            }

            var diver = new Diver(user, request.FirstName, request.LastName);
            await AutoConfirmEmailAddress(user);
            await userManager.AddToRoleAsync(user, Rolenames.Tauchbold);

            await diverRepository.InsertAsync(diver);

            var warningMessage = "";
            if (user.LockoutEnabled)
            {
                logger.LogWarning("User [{username}] is locked out!", request.UserName);
                warningMessage += "Mitglied ist noch gesperrt (LockoutEnabled). ";
            }
            if (!user.EmailConfirmed)
            {
                logger.LogWarning("User [{username}] has not confirmed email address yet!", request.UserName);
                warningMessage += "Mitglied hat seine Emailadresse noch nicht bestätigt!";
            }
            
            logger.LogInformation("User [{username}] added as a member.", request.UserName);
            
            return UseCaseResult<string>.Success(warningMessage.Trim());
        }

        private async Task AutoConfirmEmailAddress(IdentityUser user)
        {
            var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, emailConfirmToken);
        }
    }
}