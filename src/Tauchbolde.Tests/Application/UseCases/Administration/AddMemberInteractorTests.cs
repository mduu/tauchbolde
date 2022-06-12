using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Administration.AddMemberUseCase;
using Tauchbolde.SharedKernel;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Administration
{
    public class AddMemberInteractorTests
    {
        private readonly AddMemberInteractor interactor;
        private readonly ILogger<AddMemberInteractor> logger = A.Fake<ILogger<AddMemberInteractor>>();
        private readonly IDiverRepository diverRepository = DiverRepositoryFactory.CreateRepository();
        private readonly UserManager<IdentityUser> userManager = A.Fake<UserManager<IdentityUser>>();
        private readonly ICurrentUser currentUser = A.Fake<ICurrentUser>();

        public AddMemberInteractorTests()
        {
            A.CallTo(() => userManager.FindByNameAsync(A<string>._))
                .ReturnsLazily(() => Task.FromResult(new IdentityUser(DiverFactory.JohnDoeUserName)));
            
            interactor = new AddMemberInteractor(logger, diverRepository, userManager, currentUser);
        }

        [Fact]
        public async Task Handle_IsAdmin_Success()
        {
            // Arrange
            A.CallTo(() => currentUser.GetIsAdminAsync())
                .ReturnsLazily(() => Task.FromResult(true));

            // Act
            var useCaseResult = await interactor.Handle(CreateAddMemberRequest(), CancellationToken.None);
            
            // Assert
            useCaseResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_IsNotAdmin_MustDenyAccess()
        {
            // Arrange
            A.CallTo(() => currentUser.GetIsAdminAsync())
                .ReturnsLazily(() => Task.FromResult(false));

            // Act
            var useCaseResult = await interactor.Handle(CreateAddMemberRequest(), CancellationToken.None);
            
            // Assert
            useCaseResult.IsSuccessful.Should().BeFalse();
            useCaseResult.ResultCategory.Should().Be(ResultCategory.AccessDenied);
        }

        [Fact]
        public async Task Handle_InvalidUserName_MustFail()
        {
            // Arrange
            A.CallTo(() => currentUser.GetIsAdminAsync())
                .ReturnsLazily(() => Task.FromResult(true));
            A.CallTo(() => userManager.FindByNameAsync(A<string>._))
                .ReturnsLazily(() => Task.FromResult<IdentityUser>(null));

            // Act
            var useCaseResult = await interactor.Handle(CreateAddMemberRequest(), CancellationToken.None);
            
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

            // Assert
            act.Should().ThrowAsync<ArgumentNullException>().Result.Which.ParamName.Should().Be("request");
        }

        private static AddMember CreateAddMemberRequest() => 
            new(DiverFactory.JohnDoeUserName, DiverFactory.JohnDoeFirstName, DiverFactory.JohnDoeLastName);
    }
}