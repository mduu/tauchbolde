using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Administration.GetEditRolesUseCase;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Administration
{
    public class GetEditRolesInteractorTests
    {
        private readonly GetEditRolesInteractor interactor;
        private readonly IDiverRepository diverRepository = DiverRepositoryFactory.CreateRepository();
        private readonly UserManager<IdentityUser> userManager = A.Fake<UserManager<IdentityUser>>();
        private readonly RoleManager<IdentityRole> roleManager = A.Fake<RoleManager<IdentityRole>>();
        private readonly ICurrentUser currentUser = CurrentUserFactory.CreateCurrentUser();
        private readonly IGetEditRolesOutputPort outputPort = A.Fake<IGetEditRolesOutputPort>();

        public GetEditRolesInteractorTests()
        {
            A.CallTo(() => roleManager.Roles)
                .Returns(new List<IdentityRole>
                {
                    new(Rolenames.Administrator),
                    new(Rolenames.Tauchbold)
                }.AsQueryable());
            
            interactor = new GetEditRolesInteractor(diverRepository, userManager, roleManager, currentUser);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var request = new GetEditRoles(DiverFactory.JohnDoeUserName, outputPort);
            
            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<GetEditRolesOutput>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_DiverNotFound_MustFailWithNotFound()
        {
            // Arrange
            var request = new GetEditRoles(DiverFactory.JaneDoeUserName, outputPort);
            
            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.NotFound);
            A.CallTo(() => outputPort.Output(A<GetEditRolesOutput>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public void Handle_NullRequest_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Func<Task> act = () => interactor.Handle(null, CancellationToken.None);
            
            // Assert
            act.Should().ThrowAsync<ArgumentNullException>().Result.Which.ParamName.Should().Be("request");
        }
    }
}