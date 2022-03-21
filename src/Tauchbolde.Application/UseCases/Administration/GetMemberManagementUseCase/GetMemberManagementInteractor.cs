using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Administration.GetMemberManagementUseCase
{
    [UsedImplicitly]
    internal class GetMemberManagementInteractor : IRequestHandler<GetMemberManagement, UseCaseResult>
    {
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly IDiverRepository diverRepository;
        [NotNull] private readonly UserManager<IdentityUser> userManager;
        [NotNull] private readonly ICurrentUser currentUser;

        public GetMemberManagementInteractor(
            [NotNull] IDiverRepository diverRepository,
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] ICurrentUser currentUser,
            [NotNull] ILogger<GetMemberManagementInteractor> logger)
        {
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UseCaseResult> Handle([NotNull] GetMemberManagement request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (!await currentUser.GetIsAdminAsync())
            {
                logger.LogError("Use [{CurrentUserName}] has no access for getting member-management information!", currentUser.Username);
                return UseCaseResult.AccessDenied();
            }

            var profiles = await diverRepository.GetAllDiversAsync();
            var allMembers = await diverRepository.GetAllTauchboldeUsersAsync();
            var allUsers = 
                userManager.Users.ToList()
                .Where(u => allMembers.All(d => d.UserId != u.Id))
                .Select(u => u.UserName)
                .ToList();

            request.OutputPort?.Output(
                new MemberManagementOutput(await MapMembersAsync(profiles), allUsers));

            return UseCaseResult.Success();
        }

        private async Task<IEnumerable<MemberManagementOutput.Member>> MapMembersAsync(IEnumerable<Diver> profiles)
        {
            var result = new List<MemberManagementOutput.Member>();
            foreach (var profile in profiles)
            {
                result.Add(new MemberManagementOutput.Member(
                    profile.Id,
                    profile.Fullname,
                    profile.User.UserName,
                    profile.User.Email,
                    profile.User.EmailConfirmed,
                    profile.User.LockoutEnabled,
                    await userManager.GetRolesAsync(profile.User)));
            }

            return result;
        }
    }
}