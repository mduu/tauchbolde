using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Profile.MemberListUseCase;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Profile
{
    public class MemberListInteractorTests
    {
        [NotNull] private readonly MemberListInteractor interactor;
        [NotNull] private readonly ILogger<MemberListInteractor> logger = A.Fake<ILogger<MemberListInteractor>>();
        [NotNull] private readonly IDiverRepository diverRepository = DiverRepositoryFactory.CreateRepository();
        [NotNull] private readonly ICurrentUser currentUser = A.Fake<ICurrentUser>();
        [NotNull] private readonly IMemberListOutputPort outputPort = A.Fake<IMemberListOutputPort>();

        public MemberListInteractorTests()
        {
            interactor = new MemberListInteractor(logger, diverRepository, currentUser);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public async Task Handle_Success(bool isTauchboldOrAdmin, bool expectedAllowSeeDetails)
        {
            // Arrange
            A.CallTo(() => currentUser.GetIsTauchboldOrAdminAsync())
                .ReturnsLazily(() => Task.FromResult(isTauchboldOrAdmin));

            // Act
            var useCaseResult = await interactor.Handle(CreateMemberListRequest(), CancellationToken.None);

            // Assert
            useCaseResult.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<MemberListOutput>.That.Matches(o =>
                    o.AllowSeeDetails == expectedAllowSeeDetails &&
                    o.Members.Any())))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Handle_NullNotification_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Func<Task> act = () => interactor.Handle(null, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ArgumentNullException>().Result.Which.ParamName.Should().Be("request");
        }

        private MemberList CreateMemberListRequest() => new(outputPort);
    }
}