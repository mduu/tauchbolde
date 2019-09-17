using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Event.EditEventUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Event
{
    public class EditEventInteractorTests
    {
        private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
        private readonly IDiverRepository diverRepository = A.Fake<IDiverRepository>();
        private readonly ILogger<EditEventInteractor> logger = A.Fake<ILogger<EditEventInteractor>>();
        private readonly EditEventInteractor interactor;
        private readonly Guid validEventId = new Guid("74FC068A-2A97-48C1-AD40-07DAB50F56E8");
        private readonly string validUserName = "john.doe";
        private readonly Guid validOrganizatorId = new Guid("C2E349C7-E2FF-4E39-97A4-21486AF0F067");

        public EditEventInteractorTests()
        {
            A.CallTo(() => diverRepository.FindByUserNameAsync(A<string>._))
                .ReturnsLazily(call => Task.FromResult(
                    (string) call.Arguments[0] == validUserName
                        ? new Diver {Id = validOrganizatorId}
                        : null));

            A.CallTo(() => eventRepository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validEventId
                        ? new Tauchbolde.Domain.Entities.Event
                        {
                            Id = validEventId,
                            Name = "Event",
                            Location = "Old Location",
                            MeetingPoint = "Old MeetingPoint",
                            Description = "Old Description",
                            OrganisatorId = validOrganizatorId,
                        }
                        : null));

            interactor = new EditEventInteractor(logger, eventRepository, diverRepository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var request = CreateEditEvent();

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_OrginzatorNotMatching_MustFail()
        {
            // Arrange
            var request = CreateEditEvent();
            A.CallTo(() => diverRepository.FindByUserNameAsync(A<string>._))
                .ReturnsLazily(call => Task.FromResult(
                    (string) call.Arguments[0] == validUserName
                        ? new Diver {Id = new Guid("07E86139-3F98-46B3-94AD-AF427B6933AC")}
                        : null));

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.AccessDenied);
        }

        [Fact]
        public async Task Handle_DiverIdNotFound_MustFail()
        {
            // Arrange
            var request = CreateEditEvent(currentUserName: "jane.doe");

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.NotFound);
        }

        [Fact]
        public async Task Handle_EventIdNotFound_MustCreateNewEvent()
        {
            // Arrange
            var request = CreateEditEvent(eventId: new Guid("3FA9F128-A5A7-47A8-83D6-83BDF5A77A92"));

            // Act
            var result = await interactor.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.Should().BeTrue();
            A.CallTo(() => eventRepository.InsertAsync(A<Tauchbolde.Domain.Entities.Event>._))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => eventRepository.UpdateAsync(A<Tauchbolde.Domain.Entities.Event>._))
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

        private EditEvent CreateEditEvent(
            string currentUserName = null,
            Guid? eventId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string title = "Test edited Event",
            string location = "New Location",
            string meetingPoint = "New MeetingPoint",
            string description = "New Description") =>
            new EditEvent(
                currentUserName ?? validUserName,
                eventId ?? validEventId,
                startTime ?? DateTime.Today,
                endTime,
                title,
                location,
                meetingPoint,
                description);
    }
}