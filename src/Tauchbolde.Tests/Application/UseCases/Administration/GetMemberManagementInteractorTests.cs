using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Administration.GetMemberManagementUseCase;
using Tauchbolde.SharedKernel;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Administration
{
    public class GetMemberManagementInteractorTests
    {
        private readonly GetMemberManagementInteractor interactor;
        private readonly IDiverRepository diverRepository = DiverRepositoryFactory.CreateRepository();
        private readonly UserManager<IdentityUser> userManager = A.Fake<UserManager<IdentityUser>>();
        private readonly ICurrentUser currentUser = A.Fake<ICurrentUser>();
        private readonly ILogger<GetMemberManagementInteractor> logger = A.Fake<ILogger<GetMemberManagementInteractor>>();
        private readonly IMemberManagementOutputPort presenter = A.Fake<IMemberManagementOutputPort>();

        public GetMemberManagementInteractorTests()
        {
            A.CallTo(() => currentUser.GetIsAdminAsync())
                .ReturnsLazily(() => Task.FromResult(true));

            A.CallTo(() => userManager.Users)
                .ReturnsLazily(() =>
                    new List<IdentityUser>
                    {
                        new IdentityUser(DiverFactory.JohnDoeUserName),
                        new IdentityUser(DiverFactory.JaneDoeUserName)
                    }.AsQueryable());

            interactor = new GetMemberManagementInteractor(diverRepository, userManager, currentUser, logger);
        }

        [Fact]
        public async Task Handle_IsAdmin_Success()
        {
            // Arrange
            var request = new GetMemberManagement(presenter);

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => presenter.Output(A<MemberManagementOutput>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_NotIsAdmin_DenyAccess()
        {
            // Arrange
            A.CallTo(() => currentUser.GetIsAdminAsync())
                .ReturnsLazily(() => Task.FromResult(false));
            var request = new GetMemberManagement(presenter);

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.AccessDenied);
            A.CallTo(() => presenter.Output(A<MemberManagementOutput>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public void Handle_NulLRequest_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Func<Task> act = () => interactor.Handle(null, CancellationToken.None);

            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("request");
        }
    }
}