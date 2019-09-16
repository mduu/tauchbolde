using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Event.GetEventNewDetailsUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class GetEventNewDetailsInteractorTests
    {
        private const string ValidUserName = "joe.doe";
        private readonly GetEventNewDetailsInteractor interactor;
        private readonly ILogger<GetEventNewDetailsInteractor> logger = A.Fake<ILogger<GetEventNewDetailsInteractor>>();
        private readonly IDiverRepository diverRepository = A.Fake<IDiverRepository>();
        private readonly IClock clock = A.Fake<IClock>();
        private readonly IGetEventNewDetailsOutputPort outputPort = A.Fake<IGetEventNewDetailsOutputPort>();


        public GetEventNewDetailsInteractorTests()
        {
            A.CallTo(() => clock.Now()).Returns(
                new DateTime(2019, 9, 16, 12, 0, 0));

            A.CallTo(() => diverRepository.FindByUserNameAsync(A<string>._))
                .ReturnsLazily(call => Task.FromResult(
                    (string) call.Arguments[0] == ValidUserName
                        ? new Diver
                        {
                            Id = new Guid("34B6AF4A-C944-4F12-93DC-993C019718A6"),
                            User = new IdentityUser(ValidUserName)
                            {
                                Email = "john.doe@company.com"
                            },
                            AvatarId = "john_doe_1.jpg",
                            Fullname = "John Doe",
                        }
                        : null));

            interactor = new GetEventNewDetailsInteractor(logger, diverRepository, clock);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var request = new GetEventNewDetails(ValidUserName, outputPort);
        
            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<GetEventNewOutput>._)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public async Task Handle_InvalidCurrentUserName_MustFail()
        {
            // Arrange
            var request = new GetEventNewDetails("jane.doe", outputPort);
        
            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.NotFound);
            A.CallTo(() => outputPort.Output(A<GetEventNewOutput>._)).MustNotHaveHappened();
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
    }
}