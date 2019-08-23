using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.OldDomainServices.Notifications;
using Tauchbolde.Application.Services;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;
using Xunit;

namespace Tauchbolde.Tests.Application.OldDomainServices.Notifications
{
    public class NotificationServiceTests
    {
        private Guid johnDiverId = new Guid("c7c1761f-318f-4a51-ba2c-4368526182c7");
        private Guid janeDiverId = new Guid("604f5dd8-7985-45c4-b0e1-31cdacfee2ad");

        [Theory]
        [InlineData(false, 1)]
        [InlineData(true, 2)]
        public async Task NotifyForNewEventAsync(bool sendOwnNotifications, int expectedNotificationInserts)
        {
            // Arrange
            var currentUser = CreateCurrentUser(sendOwnNotifications);
            var notificationRepo = CreateNotificationRepo();
            var notificationService = CreateNotificationService(notificationRepo, sendOwnNotifications);
            var evt = new Event
            {
                Id = new Guid("b4de413a-7d40-4319-80b3-517d22a9247d"),
                Name = "Test Event",
                OrganisatorId = johnDiverId,
                Organisator = currentUser,
            };

            // Act
            await notificationService.NotifyForNewEventAsync(evt, currentUser);

            // Assert
            A.CallTo(() => notificationRepo.InsertAsync(A<Notification>._))
                .MustHaveHappened(expectedNotificationInserts, Times.Exactly);
        }

        [Theory]
        [InlineData(false, 1)]
        [InlineData(true, 2)]
        public async Task NotifyForChangedEventAsync(bool sendOwnNotifications, int expectedNotificationInserts)
        {
            // Arrange
            var currentUser = CreateCurrentUser(sendOwnNotifications);
            var evt = new Event
            {
                Id = new Guid("b4de413a-7d40-4319-80b3-517d22a9247d"),
                Name = "Test Event",
                OrganisatorId = johnDiverId,
                Organisator = currentUser,
            };
            var notificationRepo = CreateNotificationRepo();
            var notificationService = CreateNotificationService(notificationRepo, sendOwnNotifications);

            // Act
            await notificationService.NotifyForChangedEventAsync(evt, currentUser);

            // Assert
            A.CallTo(() => notificationRepo.InsertAsync(A<Notification>._))
                .MustHaveHappened(expectedNotificationInserts, Times.Exactly);
        }

        [Theory]
        [InlineData(false, 1)]
        [InlineData(true, 2)]
        public async Task NotifyForChangedParticipationAsync(bool sendOwnNotifications, int expectedNotificationInserts)
        {
            // Arrange
            var participant = new Participant
            {
                Id = new Guid("d23e977e-9b33-44c2-aa5e-1595cde11082"),
                ParticipatingDiver = CreateDiverJane(sendOwnNotifications),
            };
            var notificationRepo = CreateNotificationRepo();
            var notificationService = CreateNotificationService(notificationRepo, sendOwnNotifications);

            // Act
            await notificationService.NotifyForChangedParticipationAsync(participant, participant.ParticipatingDiver,
                participant.EventId);

            // Assert
            A.CallTo(() => notificationRepo.InsertAsync(A<Notification>._))
                .MustHaveHappened(expectedNotificationInserts, Times.Exactly);
        }

        [Theory]
        [InlineData(false, 1)]
        [InlineData(true, 2)]
        public async Task NotifyForChangedParticipationCancel(bool sendOwnNotifications,
            int expectedNotificationInsertsPhase)
        {
            // Arrange
            var participant = new Participant
            {
                Id = new Guid("d23e977e-9b33-44c2-aa5e-1595cde11082"),
                ParticipatingDiver = CreateDiverJane(sendOwnNotifications),
                Status = ParticipantStatus.Declined,
            };
            var participantRepo = CreateParticipantRepo();
            A.CallTo(() => participantRepo.GetParticipantsForEventByStatusAsync(A<Guid>._, ParticipantStatus.Declined))
                .ReturnsLazily(async (c) => await Task.FromResult(new[] {participant}));

            var notificationRepo = CreateNotificationRepo();

            var notificationService =
                CreateNotificationService(notificationRepo, sendOwnNotifications, participantRepo);

            // Act
            await notificationService.NotifyForChangedParticipationAsync(participant, participant.ParticipatingDiver,
                participant.EventId);

            A.CallTo(() => notificationRepo.InsertAsync(A<Notification>._))
                .MustHaveHappened(expectedNotificationInsertsPhase, Times.Exactly);
        }

        private static INotificationRepository CreateNotificationRepo() => A.Fake<INotificationRepository>();

        private NotificationService CreateNotificationService(
            INotificationRepository notificationRepo,
            bool sendOwnNotifications = false,
            IParticipantRepository participantRepo = null)
        {
            var diverRepo = CreateDiverRepoFakeWithTauchbolde(sendOwnNotifications);
            var eventRepo = A.Fake<IEventRepository>();

            return new NotificationService(
                notificationRepo,
                diverRepo,
                participantRepo ?? CreateParticipantRepo(),
                eventRepo,
                A.Fake<ITelemetryService>()
            );
        }

        private IParticipantRepository CreateParticipantRepo() => A.Fake<IParticipantRepository>();

        private IDiverRepository CreateDiverRepoFakeWithTauchbolde(bool sendOwnNotifications = false)
        {
            var diverRepo = A.Fake<IDiverRepository>();

            A.CallTo(() => diverRepo.GetAllTauchboldeUsersAsync(false)).ReturnsLazily((call) =>
                Task.FromResult<ICollection<Diver>>(
                    new[]
                    {
                        CreateDiverJohn(sendOwnNotifications),
                        CreateDiverJane(sendOwnNotifications),
                    }
                ));

            return diverRepo;
        }

        private Diver CreateCurrentUser(bool sendOwnNotifications = false) => CreateDiverJohn(sendOwnNotifications);

        private Diver CreateDiverJohn(bool sendOwnNotifications = false) =>
            CreateDiver(johnDiverId, "John", "Doe", sendOwnNotifications);

        private Diver CreateDiverJane(bool sendOwnNotifications = false) =>
            CreateDiver(janeDiverId, "Jane", "Doe", sendOwnNotifications);

        private Diver CreateDiver(Guid diverId, string firstname, string lastname, bool sendOwnNotifications = false)
            => new Diver
            {
                Id = diverId,
                Firstname = firstname,
                Lastname = lastname,
                Fullname = $"{firstname} {lastname}",
                SendOwnNoticiations = sendOwnNotifications,
                User = new IdentityUser
                {
                    Id = $"{firstname}.{lastname}",
                    Email = $"{firstname}.{lastname}@company.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                }
            };
    }
}