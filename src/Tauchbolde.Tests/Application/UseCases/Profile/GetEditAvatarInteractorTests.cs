using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Profile.GetEditAvatarUseCase;
using Tauchbolde.SharedKernel;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Profile
{
    public class GetEditAvatarInteractorTests
    {
        [NotNull] private readonly GetEditAvatarInteractor interactor;
        [NotNull] private readonly ILogger<GetEditAvatarInteractor> logger = A.Fake<ILogger<GetEditAvatarInteractor>>();
        [NotNull] private readonly IDiverRepository diverRepository = A.Fake<IDiverRepository>();
        [NotNull] private readonly ICurrentUser currentUser = A.Fake<ICurrentUser>();
        [NotNull] private readonly IGetEditAvatarOutputPort outputPort = A.Fake<IGetEditAvatarOutputPort>();
        [NotNull] private readonly GetEditAvatar editJohnDoeAvatarRequest;

        public GetEditAvatarInteractorTests()
        {
            editJohnDoeAvatarRequest = new GetEditAvatar(DiverFactory.JohnDoeDiverId, outputPort);
            
            A.CallTo(() => diverRepository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == DiverFactory.JohnDoeDiverId
                        ? DiverFactory.CreateJohnDoe()
                        : null));
            
            interactor = new GetEditAvatarInteractor(logger, diverRepository, currentUser);
        }

        [Fact]
        public async Task Handle_CurrentDiver_Success()
        {
            // Arrange
            A.CallTo(() => currentUser.GetCurrentDiverAsync())
                .ReturnsLazily(() => Task.FromResult(DiverFactory.CreateJohnDoe()));

            // Act
            var result = await interactor.Handle(editJohnDoeAvatarRequest, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<GetEditAvatarOutput>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_OtherDiver_FailWithAccessDenied()
        {
            // Arrange
            A.CallTo(() => currentUser.GetCurrentDiverAsync())
                .ReturnsLazily(() => Task.FromResult(DiverFactory.CreateJaneDoe()));

            // Act
            var result = await interactor.Handle(editJohnDoeAvatarRequest, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.AccessDenied);
            A.CallTo(() => outputPort.Output(A<GetEditAvatarOutput>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task Handle_Admins_Success()
        {
            // Arrange
            A.CallTo(() => currentUser.GetCurrentDiverAsync())
                .ReturnsLazily(() => Task.FromResult(DiverFactory.CreateJaneDoe()));
            A.CallTo(() => currentUser.GetIsAdminAsync())
                .ReturnsLazily(() => Task.FromResult(true));

            // Act
            var result = await interactor.Handle(editJohnDoeAvatarRequest, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<GetEditAvatarOutput>._))
                .MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task Handle_UnknownDiver_FailWithNotFound()
        {
            // Arrange
            A.CallTo(() => currentUser.GetCurrentDiverAsync())
                .ReturnsLazily(() => Task.FromResult(DiverFactory.CreateJohnDoe()));
            var request = new GetEditAvatar(DiverFactory.JaneDoeDiverId, outputPort);

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.NotFound);
            A.CallTo(() => outputPort.Output(A<GetEditAvatarOutput>._))
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