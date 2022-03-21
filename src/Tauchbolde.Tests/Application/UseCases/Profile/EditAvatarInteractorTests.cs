using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services;
using Tauchbolde.Application.Services.Avatars;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Profile.EditAvatarUseCase;
using Tauchbolde.SharedKernel;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Profile
{
    public class EditAvatarInteractorTests
    {
        [NotNull] private readonly EditAvatarInteractor interactor;
        [NotNull] private readonly IDiverRepository diverRepository = DiverRepositoryFactory.CreateRepository();
        [NotNull] private readonly IAvatarStore avatarStore = A.Fake<IAvatarStore>();
        [NotNull] private readonly ICurrentUser currentUser = CurrentUserFactory.CreateCurrentUser();
        [NotNull] private readonly EditAvatar.AvatarFile validAvatarFile = new(
            "john1.jpg",
            "image/jpeg",
            new MemoryStream());

        public EditAvatarInteractorTests()
        {
            interactor = new EditAvatarInteractor(
                A.Fake<ILogger<EditAvatarInteractor>>(),
                diverRepository,
                avatarStore,
                new MimeMapping(),
                currentUser);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var request = new EditAvatar(DiverFactory.JohnDoeDiverId, validAvatarFile);
   
            // Act
            var useCaseResult = await interactor.Handle(request, CancellationToken.None);

            // Assert
            useCaseResult.IsSuccessful.Should().BeTrue();
            A.CallTo(() => avatarStore.StoreAvatarAsync(
                DiverFactory.JohnDoeFirstName,
                A<string>._,
                A<string>._,
                A<Stream>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_DiverNotFound_MustReturnNotFound()
        {
            // Arrange
            var request = new EditAvatar(DiverFactory.JaneDoeDiverId, validAvatarFile);
   
            // Act
            var useCaseResult = await interactor.Handle(request, CancellationToken.None);

            // Assert
            useCaseResult.IsSuccessful.Should().BeFalse();
            useCaseResult.ResultCategory.Should().Be(ResultCategory.NotFound);
            A.CallTo(() => avatarStore.StoreAvatarAsync(
                DiverFactory.JohnDoeFirstName,
                A<string>._,
                A<string>._,
                A<Stream>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task Handle_NotCurrentDiverAndNotAdmin_MustReturnAccessDenied()
        {
            // Arrange
            var request = new EditAvatar(DiverFactory.JaneDoeDiverId, validAvatarFile);
            A.CallTo(() => diverRepository.FindByIdAsync(DiverFactory.JaneDoeDiverId))
                .ReturnsLazily(() => Task.FromResult(DiverFactory.CreateJaneDoe()));
   
            // Act
            var useCaseResult = await interactor.Handle(request, CancellationToken.None);

            // Assert
            useCaseResult.IsSuccessful.Should().BeFalse();
            useCaseResult.ResultCategory.Should().Be(ResultCategory.AccessDenied);
            A.CallTo(() => avatarStore.StoreAvatarAsync(
                DiverFactory.JohnDoeFirstName,
                A<string>._,
                A<string>._,
                A<Stream>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task Handle_NotCurrentDiverButAdmin_MustSucceed()
        {
            // Arrange
            var request = new EditAvatar(DiverFactory.JaneDoeDiverId, validAvatarFile);
            A.CallTo(() => diverRepository.FindByIdAsync(DiverFactory.JaneDoeDiverId))
                .ReturnsLazily(() => Task.FromResult(DiverFactory.CreateJaneDoe()));
            A.CallTo(() => currentUser.GetIsDiverOrAdmin(A<Guid>._))
                .ReturnsLazily(() => Task.FromResult(true));
   
            // Act
            var useCaseResult = await interactor.Handle(request, CancellationToken.None);

            // Assert
            useCaseResult.IsSuccessful.Should().BeTrue();
            A.CallTo(() => avatarStore.StoreAvatarAsync(
                DiverFactory.JaneDoeFirstName,
                A<string>._,
                A<string>._,
                A<Stream>._)).MustHaveHappenedOnceExactly();
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