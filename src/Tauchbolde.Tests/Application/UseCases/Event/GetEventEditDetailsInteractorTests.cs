using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Event.GetEventEditDetailsUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class GetEventEditDetailsInteractorTests
    {
        private readonly string validUserName = "john.doe";
        private readonly Guid validDiverId = new Guid("591FC123-5F37-42BC-B7A9-0A1E86C22C7C");
        private readonly Guid validEventId = new Guid("125EB70A-69D7-4B9E-8ADB-D0A8FAA10BAA");
        private readonly GetEventEditDetailsInteractor interactor;
        private readonly ILogger<GetEventEditDetailsInteractor> logger = A.Fake<ILogger<GetEventEditDetailsInteractor>>();
        private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
        private readonly IDiverRepository diverRepository = A.Fake<IDiverRepository>();
        private readonly IEventEditDetailsOutputPort outputPort = A.Fake<IEventEditDetailsOutputPort>();
        private readonly IClock clock = A.Fake<IClock>();
        private readonly ICurrentUser currentUser = A.Fake<ICurrentUser>();

        public GetEventEditDetailsInteractorTests()
        {
            A.CallTo(() => diverRepository.FindByUserNameAsync(A<string>._))
                .ReturnsLazily(call => Task.FromResult(
                    (string) call.Arguments[0] == validUserName
                        ? CreateDiverJohnDoe()
                        : null));

            A.CallTo(() => eventRepository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validEventId
                        ? new Tauchbolde.Domain.Entities.Event
                        {
                            Id = validEventId,
                            Name = "Test Event",
                            Location = "",
                            MeetingPoint = "",
                            Description = "",
                            OrganisatorId = validDiverId,
                            Organisator = CreateDiverJohnDoe()
                        }
                        : null));

            A.CallTo(() => clock.Now()).Returns(new DateTime(2019, 9, 1, 19, 0, 0));

            A.CallTo(() => currentUser.GetCurrentDiver())
                .ReturnsLazily(() => Task.FromResult(
                    new Diver
                    {
                        Id = validDiverId,
                        Fullname = "Joe Doe",
                        User = new IdentityUser("joe.doe")
                        {
                            Email = "joe.doe@company.com"
                        }
                    }));

            interactor = new GetEventEditDetailsInteractor(logger, eventRepository, clock, currentUser);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var request = new GetEventEditDetails(validEventId, outputPort);

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<EventEditDetailsOutput>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_InvalidDiverId_MustFail()
        {
            // Arrange
            A.CallTo(() => currentUser.GetCurrentDiver())
                .ReturnsLazily(() => Task.FromResult<Diver>(null));
            var request = new GetEventEditDetails(validEventId, outputPort);

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.NotFound);
            A.CallTo(() => outputPort.Output(A<EventEditDetailsOutput>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task Handle_InvalidEventId_MustReturnNew()
        {
            // Arrange
            var request = new GetEventEditDetails(new Guid("C2B9BB52-BFB0-4B43-965C-C23B4598722B"), outputPort);

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<EventEditDetailsOutput>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_NotOrganisatorUser_MustFail()
        {
            // Arrange
            var request = new GetEventEditDetails(validEventId, outputPort);
            A.CallTo(() => currentUser.GetCurrentDiver())
                .ReturnsLazily(() => Task.FromResult(
                    new Diver
                    {
                        Id = new Guid("F9D10AB9-A076-448A-9391-755122FC01B8"),
                        Fullname = "Jane Doe",
                    }));

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.AccessDenied);
            A.CallTo(() => outputPort.Output(A<EventEditDetailsOutput>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public void Handle_NullRequest_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Func<Task> act = () => interactor.Handle(null, CancellationToken.None);

            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("request");
        }

        private Diver CreateDiverJohnDoe() =>
            new Diver
            {
                Id = validDiverId,
                Fullname = "John Doe",
                User = new IdentityUser(validUserName) {Email = "john.doe@company.com"},
                AvatarId = "johns_avatar"
            };
    }
}