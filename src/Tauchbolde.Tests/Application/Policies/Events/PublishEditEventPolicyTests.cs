using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Policies.Event.EventEdited;
using Tauchbolde.Application.Services.Notifications;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Events.Event;
using Tauchbolde.Domain.Types;
using Xunit;

namespace Tauchbolde.Tests.Application.Policies.Events
{
    public class PublishEditEventPolicyTests
    {
        [NotNull] private readonly PublishEditEventPolicy policy;
        [NotNull] private readonly ILogger<PublishEditEventPolicy> logger = A.Fake<ILogger<PublishEditEventPolicy>>();
        [NotNull] private readonly INotificationPublisher notificationPublisher = A.Fake<INotificationPublisher>();
        [NotNull] private readonly IRecipientsBuilder recipientsBuilder = A.Fake<IRecipientsBuilder>();
        [NotNull] private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
        private readonly Guid validEventId = new Guid("A870DCC0-0F50-48B9-8FCC-1A62365D51CF");
        private readonly Guid currentUserId = new Guid("97359838-63F0-4A9E-94F1-2B01DB49349F");

        public PublishEditEventPolicyTests()
        {
            A.CallTo(() => eventRepository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validEventId
                        ? new Event
                        {
                            Id = validEventId,
                            Name = "Test Event",
                            Location = "loc",
                            MeetingPoint = "meet",
                            Description = "descr",
                            StartTime = new DateTime(2019, 9, 1, 19, 0, 0),
                            OrganisatorId = currentUserId,
                            Organisator = new Diver
                            {
                                Id = new Guid("8DCF530A-E55D-4FED-97D7-D636CDAD8BFC"),
                                Fullname = "John Doe",
                            }
                        }
                        : null));

            A.CallTo(() => recipientsBuilder.GetAllTauchboldeButDeclinedParticipantsAsync(currentUserId, validEventId))
                .ReturnsLazily(() => Task.FromResult(
                    new List<Diver>
                    {
                        new Diver { Id = new Guid("A73286C3-BD30-4D2B-BAF4-43F7A0EB7B21")},
                        new Diver { Id = new Guid("6D717184-7A4B-4091-9133-AC539F55C5C4")}
                    }));

            policy = new PublishEditEventPolicy(logger, notificationPublisher, recipientsBuilder, eventRepository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            EventEditedEvent notification = CreateNotification();
           
            // Act
            await policy.Handle(notification, CancellationToken.None);

            // Assert
            A.CallTo(() => notificationPublisher.PublishAsync(
                    NotificationType.EditEvent,
                    A<string>._,
                    A<IEnumerable<Diver>>._,
                    A<Diver>._,
                    validEventId,
                    null))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_InvalidEventId_MustFail()
        {
            // Arrange
            EventEditedEvent notification = CreateNotification(eventId: new Guid("56C45285-782B-4C23-9E10-4877240232E0"));
           
            // Act
            await policy.Handle(notification, CancellationToken.None);

            // Assert
            A.CallTo(() => notificationPublisher.PublishAsync(
                    NotificationType.EditEvent,
                    A<string>._,
                    A<IEnumerable<Diver>>._,
                    A<Diver>._,
                    A<Guid>._,
                    A<Guid?>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public void Handle_NullNotification_MustThrow()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Func<Task> act = () => policy.Handle(null, CancellationToken.None);
            
            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("notification");
        }

        private EventEditedEvent CreateNotification(Guid? eventId = null) => 
            new EventEditedEvent(eventId ?? validEventId);
    }
}