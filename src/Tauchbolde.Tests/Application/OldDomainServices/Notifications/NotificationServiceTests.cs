using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.OldDomainServices.Notifications;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Entities;
using Xunit;

namespace Tauchbolde.Tests.Application.OldDomainServices.Notifications
{
    public class NotificationServiceTests
    {
        private readonly Guid johnDiverId = new Guid("c7c1761f-318f-4a51-ba2c-4368526182c7");
        private readonly Guid janeDiverId = new Guid("604f5dd8-7985-45c4-b0e1-31cdacfee2ad");

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
        
        private static INotificationRepository CreateNotificationRepo() => A.Fake<INotificationRepository>();

        private NotificationService CreateNotificationService(
            INotificationRepository notificationRepo,
            bool sendOwnNotifications = false)
        {
            var diverRepo = CreateDiverRepoFakeWithTauchbolde(sendOwnNotifications);

            return new NotificationService(
                notificationRepo,
                diverRepo,
                A.Fake<ITelemetryService>()
            );
        }

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