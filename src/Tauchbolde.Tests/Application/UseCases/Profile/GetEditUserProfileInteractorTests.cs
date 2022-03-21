using System;
using System.Threading;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Profile.GetEditUserProfileUseCase;
using Tauchbolde.SharedKernel;
using Tauchbolde.Tests.TestingTools.TestDataFactories;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Profile
{
    [UseReporter((typeof(DiffReporter)))]
    public class GetEditUserProfileInteractorTests
    {
        private readonly GetEditUserProfileInteractor interactor;
        private readonly IDiverRepository diverRepository = A.Fake<IDiverRepository>();
        private readonly UserManager<IdentityUser> userManager = A.Fake<UserManager<IdentityUser>>();
        private readonly ICurrentUser currentUser = A.Fake<ICurrentUser>();
        private readonly IGetEditUserProfileOutputPort outputPort = A.Fake<IGetEditUserProfileOutputPort>();

        public GetEditUserProfileInteractorTests()
        {
            var logger = A.Fake<ILogger<GetEditUserProfileInteractor>>();

            A.CallTo(() => diverRepository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == DiverFactory.JohnDoeDiverId
                        ? DiverFactory.CreateJohnDoe()
                        : null));

            interactor = new GetEditUserProfileInteractor(logger, diverRepository, userManager, currentUser);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            GetEditUserProfileOutput recordedOutput = null;
            A.CallTo(() => outputPort.Output(A<GetEditUserProfileOutput>._))
                .Invokes(call => recordedOutput = (GetEditUserProfileOutput) call.Arguments[0]);

            // Act
            var result = await interactor.Handle(CreateRequest(DiverFactory.JohnDoeDiverId), CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<GetEditUserProfileOutput>._))
                .MustHaveHappenedOnceExactly();
            Approvals.VerifyJson(JsonConvert.SerializeObject(recordedOutput));
        }

        [Fact]
        public async Task Handle_InvalidDiver_MustReturnNotFound()
        {
            // Arrange
            
            // Act
            var result = await interactor.Handle(CreateRequest(DiverFactory.JaneDoeDiverId), CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.NotFound);
            A.CallTo(() => outputPort.Output(A<GetEditUserProfileOutput>._))
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

        private GetEditUserProfile CreateRequest(Guid diverId) => new(diverId, outputPort);
    }
}