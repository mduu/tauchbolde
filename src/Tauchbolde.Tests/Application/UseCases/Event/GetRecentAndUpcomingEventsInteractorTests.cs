using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Event.GetRecentAndUpcomingEventsUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class GetRecentAndUpcomingEventsInteractorTests
    {
        private readonly GetRecentAndUpcomingEventsInteractor interactor;
        private readonly IEventRepository repository = A.Fake<IEventRepository>();
        private readonly IRecentAndUpcomingEventsOutputPort outputPort = A.Fake<IRecentAndUpcomingEventsOutputPort>();

        public GetRecentAndUpcomingEventsInteractorTests()
        {
            A.CallTo(() => repository.GetUpcomingAndRecentEventsAsync())
                .ReturnsLazily(() => Task.FromResult<ICollection<Tauchbolde.Domain.Entities.Event>>(
                    new List<Tauchbolde.Domain.Entities.Event>
                    {
                        new Tauchbolde.Domain.Entities.Event {Id = new Guid("59D6EA6F-1863-4372-9A47-35D5B7533826")},
                        new Tauchbolde.Domain.Entities.Event {Id = new Guid("1B930343-C474-4B27-A1E0-E8DE192B33FF")}
                    }));

            interactor = new GetRecentAndUpcomingEventsInteractor(repository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            GetRecentAndUpcomingEvents request = new GetRecentAndUpcomingEvents(outputPort);
            
            // Act
            var result = await interactor.Handle(request, CancellationToken.None);
            
            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => outputPort.Output(A<RecentAndUpcomingEventsOutput>._))
                .MustHaveHappenedOnceExactly();
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