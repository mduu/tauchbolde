using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Application.UseCases.Administration.SetUpRolesUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Administration
{
    public class SetUpRolesInteractorTests
    {
        private readonly SetUpRolesInteractor interactor;
        private readonly RoleManager<IdentityRole> rolesManager = A.Fake<RoleManager<IdentityRole>>();

        public SetUpRolesInteractorTests()
        {
            interactor = new SetUpRolesInteractor(rolesManager);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Act
            var useCaseResult = await interactor.Handle(new SetUpRoles(), CancellationToken.None);

            // Assert
            useCaseResult.IsSuccessful.Should().Be(true);
            A.CallTo(() => rolesManager.CreateAsync(A<IdentityRole>._))
                .MustHaveHappenedTwiceExactly();
        }
    }
}