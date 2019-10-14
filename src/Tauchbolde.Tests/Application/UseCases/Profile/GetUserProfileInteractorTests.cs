using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Profile;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Profile
{
    public class GetUserProfileInteractorTests
    {
        [NotNull] private readonly GetUserProfileInteractor interactor;
        [NotNull] private readonly IDiverRepository diverRepository = A.Fake<IDiverRepository>();
        [NotNull] private readonly ICurrentUser currentUser = A.Fake<ICurrentUser>();
        [NotNull] private readonly UserManager<IdentityUser> userManager = A.Fake<UserManager<IdentityUser>>();
        [NotNull] private readonly IGetUserProfileOutputPort outputPort = A.Fake<IGetUserProfileOutputPort>();

        public GetUserProfileInteractorTests()
        {
            A.CallTo(() => diverRepository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily((call) => Task.FromResult(
                    (Guid)call.Arguments[0] == DiverFactory.JohnDoeDiverId
                        ? DiverFactory.CreateJohnDoe()
                        : null));
            
            var logger = A.Fake<ILogger<GetUserProfileInteractor>>();
            interactor = new GetUserProfileInteractor(diverRepository, currentUser, userManager, logger);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var request = new GetUserProfile(DiverFactory.JohnDoeDiverId, outputPort);
            
            // Act
            var interactorResult = await interactor.Handle(request, CancellationToken.None);
            
            // Assert
            interactorResult.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<GetUserProfileOutput>.That.Matches(o => o.AllowEdit == false)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_Admin_MustBeAllowedToEdit()
        {
            // Arrange
            var request = new GetUserProfile(DiverFactory.JohnDoeDiverId, outputPort);
            A.CallTo(() => currentUser.GetIsAdminAsync())
                .ReturnsLazily(() => Task.FromResult(true));
            
            // Act
            var interactorResult = await interactor.Handle(request, CancellationToken.None);
            
            // Assert
            interactorResult.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<GetUserProfileOutput>.That.Matches(o => o.AllowEdit == true)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_OwnUser_MustBeAllowedToEdit()
        {
            // Arrange
            var request = new GetUserProfile(DiverFactory.JohnDoeDiverId, outputPort);
            A.CallTo(() => currentUser.GetCurrentDiverAsync())
                .ReturnsLazily(() => Task.FromResult(DiverFactory.CreateJohnDoe()));
            
            // Act
            var interactorResult = await interactor.Handle(request, CancellationToken.None);
            
            // Assert
            interactorResult.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<GetUserProfileOutput>.That.Matches(o => o.AllowEdit == true)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_DiverNotFound_MustReturnNotFound()
        {
            // Arrange
            var request = new GetUserProfile(DiverFactory.JaneDoeDiverId, outputPort);
            
            // Act
            var interactorResult = await interactor.Handle(request, CancellationToken.None);
            
            // Assert
            interactorResult.IsSuccessful.Should().BeFalse();
            interactorResult.ResultCategory.Should().Be(ResultCategory.NotFound);
            A.CallTo(() => outputPort.Output(A<GetUserProfileOutput>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task Handle_CurrentUserDiverNotFound_MustReturnFail()
        {
            // Arrange
            var request = new GetUserProfile(DiverFactory.JohnDoeDiverId, outputPort);
            A.CallTo(() => currentUser.GetCurrentDiverAsync())
                .ReturnsLazily(() => Task.FromResult<Diver>(null));
 
            // Act
            var interactorResult = await interactor.Handle(request, CancellationToken.None);
            
            // Assert
            interactorResult.IsSuccessful.Should().BeFalse();
            interactorResult.ResultCategory.Should().Be(ResultCategory.GeneralFailure);
            A.CallTo(() => outputPort.Output(A<GetUserProfileOutput>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public void Handle_NullRequest_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Func<Task> act = () => interactor.Handle(null, CancellationToken.None);

            // Assert
            act.Should().Throw<ArgumentException>().Which.ParamName.Should().Be("request");
        }
    }
}