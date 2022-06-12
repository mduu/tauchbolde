using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.UseCases.Administration.EditRolesUseCase;
using Tauchbolde.SharedKernel;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Administration
{
    public class EditRolesInteractorTests
    {
        private readonly EditRolesInteractor interactor;
        private readonly ILogger<EditRolesInteractor> logger = A.Fake<ILogger<EditRolesInteractor>>();
        private readonly RoleManager<IdentityRole> roleManager = A.Fake<RoleManager<IdentityRole>>();
        private readonly UserManager<IdentityUser> userManager = A.Fake<UserManager<IdentityUser>>();

        public EditRolesInteractorTests()
        {
            A.CallTo(() => userManager.FindByNameAsync(A<string>._))
                .ReturnsLazily(call => Task.FromResult(
                    (string) call.Arguments[0] == DiverFactory.JohnDoeUserName
                        ? new IdentityUser
                        {
                            UserName = DiverFactory.JohnDoeUserName,
                        }
                        : null));

            A.CallTo(() => roleManager.Roles).Returns(
                new EnumerableQuery<IdentityRole>(new List<IdentityRole>
                {
                    new("role1"),
                    new("role2"),
                    new("role3"),
                }));

            A.CallTo(() => userManager.IsInRoleAsync(A<IdentityUser>._, "role1"))
                .ReturnsLazily(() => Task.FromResult(true));
            A.CallTo(() => userManager.IsInRoleAsync(A<IdentityUser>._, "role2"))
                .ReturnsLazily(() => Task.FromResult(true));

            interactor = new EditRolesInteractor(logger, roleManager, userManager);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var request = new EditRoles(DiverFactory.JohnDoeUserName, new[] {"role2", "role3"});

            // Act
            var useCaseResult = await interactor.Handle(request, CancellationToken.None);

            // Assert
            useCaseResult.IsSuccessful.Should().BeTrue();
            A.CallTo(() => userManager.AddToRoleAsync(A<IdentityUser>._, "role3"))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.RemoveFromRoleAsync(A<IdentityUser>._, "role1"))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_InvalidUserName_MustReturnNotFound()
        {
            // Arrange
            var request = new EditRoles(DiverFactory.JaneDoeUserName, new[] {"role1"});

            // Act
            var useCaseResult = await interactor.Handle(request, CancellationToken.None);

            // Assert
            useCaseResult.IsSuccessful.Should().BeFalse();
            useCaseResult.ResultCategory.Should().Be(ResultCategory.NotFound);
        }

        [Fact]
        public void Handle_NullRequest_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Func<Task> act = () => interactor.Handle(null, CancellationToken.None);

            // Arrange
            act.Should().ThrowAsync<ArgumentNullException>().Result.Which.ParamName.Should().Be("request");
        }
    }
}