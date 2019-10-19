using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Policies.Event.ParticipationChanged;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;
using Xunit;

namespace Tauchbolde.Tests.Application.Policies.Events
{
    public class PublishParticipationChangedPolicyTests
    {
        private readonly ILogger<PublishParticipationChangedPolicy> logger = A.Fake<ILogger<PublishParticipationChangedPolicy>>();
        private readonly IDiverRepository diverRepository = A.Fake<IDiverRepository>();
        private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
        private readonly IParticipantRepository participantRepository = A.Fake<IParticipantRepository>();
        private readonly INotificationPublisher notificationPublisher = A.Fake<INotificationPublisher>();
        private readonly IRecipientsBuilder recipientsBuilder = A.Fake<IRecipientsBuilder>();
        private readonly Guid validParticipantId = new Guid("88C90CF1-FCDB-4742-A964-3D03440CF6D8");
        private readonly Guid validEventId = new Guid("FCD856A3-D672-45E8-90C7-0D43DE79A61C");
        private readonly Guid validDiverId = new Guid("6D1E557F-5C43-40FC-8BBD-74F02736305C");
        private readonly PublishParticipationChangedPolicy policy;

        private Participant foundParticipant;

        public PublishParticipationChangedPolicyTests()
        {
            foundParticipant = new Participant
            {
                Id = validParticipantId,
                EventId = validEventId,
                ParticipatingDiverId = validDiverId,
                Status = ParticipantStatus.None,
                CountPeople = 1,
                BuddyTeamName = "team1"
            };

            A.CallTo(() => participantRepository.GetParticipantByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validParticipantId
                        ? foundParticipant
                        : null
                ));

            A.CallTo(() => eventRepository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validEventId
                        ? new Event
                        {
                            Id = validEventId,
                            Name = "Testevent",
                        }
                        : null
                ));

            A.CallTo(() => diverRepository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validDiverId
                        ? new Diver { Id = validDiverId, Fullname = "John Doe" }
                        : null
                ));

            A.CallTo(() => recipientsBuilder.GetAllTauchboldeButDeclinedParticipantsAsync(validDiverId, validEventId))
                .ReturnsLazily(() => Task.FromResult(
                    new List<Diver>
                    {
                        new Diver {Id = new Guid("A00D5B04-B86D-4DAF-828F-00D286F4093B"), Fullname = "Jane Doe"},
                        new Diver {Id = new Guid("0DE86468-0F8C-4C02-A3CB-80460D45B0FF"), Fullname = "Jim Doe"},
                    }
                ));

            policy = new PublishParticipationChangedPolicy(logger, diverRepository, eventRepository, participantRepository, notificationPublisher, recipientsBuilder);
        }

        [Theory]
        [InlineData(ParticipantStatus.Accepted, NotificationType.Accepted, "John Doe nimmt an 'Testevent' teil.")]
        [InlineData(ParticipantStatus.Declined, NotificationType.Declined, "John Doe hat für 'Testevent' abgesagt.")]
        [InlineData(ParticipantStatus.Tentative, NotificationType.Tentative, "John Doe nimmt eventuell an 'Testevent' teil.")]
        [InlineData(ParticipantStatus.None, NotificationType.Neutral, "John Doe weiss nicht ob Er/Sie an 'Testevent' teil nimmt.")]
        public async Task Handle_Success(ParticipantStatus status, NotificationType expectedNotificationType, string expectedMessage)
        {
            // Arrange
            var notification = new ParticipationChangedEvent(
                validParticipantId,
                validEventId,
                validDiverId,
                ParticipantStatus.Accepted);

            A.CallTo(() => participantRepository.GetParticipantByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validParticipantId
                        ? new Participant
                        {
                            Id = validParticipantId,
                            EventId = validEventId,
                            ParticipatingDiverId = validDiverId,
                            Status = status,
                            CountPeople = 1,
                            BuddyTeamName = "team1"
                        }
                        : null
                ));

            var recordedMessage = "";
            A.CallTo(() => notificationPublisher.PublishAsync(
                expectedNotificationType,
                A<string>.That.Matches(m =>
                    !string.IsNullOrWhiteSpace(m)),
                A<IEnumerable<Diver>>.That.Matches(l => l.Count() == 2),
                A<Diver>._,
                validEventId,
                null)).Invokes(call => recordedMessage = (string) call.Arguments[1]);

            // Act
            await policy.Handle(notification, CancellationToken.None);

            // Assert
            A.CallTo(() => notificationPublisher.PublishAsync(
                    expectedNotificationType,
                    A<string>.That.Matches(m =>
                        !string.IsNullOrWhiteSpace(m)),
                    A<IEnumerable<Diver>>.That.Matches(l => l.Count() == 2),
                    A<Diver>._,
                    validEventId,
                    null))
                .MustHaveHappenedOnceExactly();
            recordedMessage.Should().Be(expectedMessage);
        }

        [Fact]
        public void Handle_ParticipantNotFound()
        {
            // Arrange
            var notification = new ParticipationChangedEvent(
                new Guid("0F5C1EE0-7EE9-43CC-B2DA-4B19568A1A3F"),
                validEventId,
                validDiverId,
                ParticipantStatus.Accepted);

            // Act
            Func<Task> act = () => policy.Handle(notification, CancellationToken.None);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Participant not found!");
        }

        [Fact]
        public void Handle_NullNotificationMustFail()
        {
            // Act
            Func<Task> act = () => policy.Handle(null, CancellationToken.None);

            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("notification");
        }

        [Fact]
        public void Handle_DiverNotFound()
        {
            // Arrange
            var notification = new ParticipationChangedEvent(
                validParticipantId,
                validEventId,
                new Guid("9B05C53E-C54F-494D-B352-CF80B1D44E97"),
                ParticipantStatus.Accepted);

            foundParticipant = new Participant
            {
                Id = notification.ParticipationId,
                ParticipatingDiverId = notification.DiverId,
                EventId = notification.EventId,
            };

            // Act
            Func<Task> act = () => policy.Handle(notification, CancellationToken.None);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Diver not found!");
        }

        [Fact]
        public void Handle_EventNotFound()
        {
            // Arrange
            var notification = new ParticipationChangedEvent(
                validParticipantId,
                new Guid("40E7F3FB-FA27-4349-9DB5-BB846D2D6DFA"),
                validDiverId,
                ParticipantStatus.Accepted);

            foundParticipant = new Participant
            {
                Id = notification.ParticipationId,
                ParticipatingDiverId = notification.DiverId,
                EventId = notification.EventId,
            };

            // Act
            Func<Task> act = () => policy.Handle(notification, CancellationToken.None);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Event not found!");
        }
    }
}