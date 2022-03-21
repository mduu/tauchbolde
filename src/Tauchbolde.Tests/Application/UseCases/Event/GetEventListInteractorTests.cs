using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Event.GetEventListUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class GetEventListInteractorTests
    {
        private readonly GetEventListInteractor interactor;
        private readonly IEventRepository repository = A.Fake<IEventRepository>();
        private readonly IEventListOutputPort outputPort = A.Fake<IEventListOutputPort>();

        public GetEventListInteractorTests()
        {
            A.CallTo(() => repository.GetUpcomingEventsAsync())
                .ReturnsLazily(() => Task.FromResult(
                    new List<Tauchbolde.Domain.Entities.Event>
                    {
                        new()
                        {
                            Id = new Guid("86B7C4ED-AB8E-4783-B93C-BD3B2AF3E071")
                        },
                        new()
                        {
                            Id = new Guid("2E0117A1-F567-40C8-AABF-E971D4948CA7")
                        },
                    }));

            interactor = new GetEventListInteractor(repository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var request = new GetEventList(outputPort);

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<GetEventListOutput>._))
                .MustHaveHappenedOnceExactly();
        }
    }
}