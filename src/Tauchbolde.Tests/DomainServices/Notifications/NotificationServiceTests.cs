using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Common.Model;
using Xunit;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Tauchbolde.Tests.DomainServices.Notifications
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
            A.CallTo(() => notificationRepo.InsertAsync(A<Notification>._)).MustHaveHappened(Repeated.Exactly.Times(expectedNotificationInserts));
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
            A.CallTo(() => notificationRepo.InsertAsync(A<Notification>._)).MustHaveHappened(Repeated.Exactly.Times(expectedNotificationInserts));
        }
        
        [Theory]
        [InlineData(false, 1)]
        [InlineData(true, 2)]
        public async Task NotifyForCanceledEventAsync(bool sendOwnNotifications, int expectedNotificationInserts)
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
            await notificationService.NotifyForCanceledEventAsync(evt, currentUser);

            // Assert
            A.CallTo(() => notificationRepo.InsertAsync(A<Notification>._)).MustHaveHappened(Repeated.Exactly.Times(expectedNotificationInserts));
        }
        
        [Theory]
        [InlineData(false, 1)]
        [InlineData(true, 2)]
        public async Task NotifyForEventCommentAsync(bool sendOwnNotifications, int expectedNotificationInserts)
        {
            // Arrange
            var author = CreateCurrentUser(sendOwnNotifications);
            var comment = new Comment
            {
                Id = new Guid("b4de413a-7d40-4319-80b3-517d22a9247d"),
                Text = "Test Event",
                AuthorId = johnDiverId,
                Author = author,
                Event = new Event
                {
                    Id = new Guid("b4de413a-7d40-4319-80b3-517d22a9247d"),
                    Name = "Test Event",
                }
            };
            var notificationRepo = CreateNotificationRepo();
            var notificationService = CreateNotificationService(notificationRepo, sendOwnNotifications);
            
            // Act
            await notificationService.NotifyForEventCommentAsync(comment, author);

            // Assert
            A.CallTo(() => notificationRepo.InsertAsync(A<Notification>._)).MustHaveHappened(Repeated.Exactly.Times(expectedNotificationInserts));
        }
        
        [Theory]
        [InlineData(false, 1)]
        [InlineData(true, 2)]
        public async Task NotifyForChangedParticipationAsync(bool sendOwnNotifications, int expectedNotificationInserts)
        {
            // Arrange
            var currentUser = CreateCurrentUser(sendOwnNotifications);
            var participant = new Participant
            {
                Id = new Guid("d23e977e-9b33-44c2-aa5e-1595cde11082"),
                ParticipatingDiver = CreateDiverJane(sendOwnNotifications),
                Event = new Event
                {
                    Id = new Guid("b4de413a-7d40-4319-80b3-517d22a9247d"),
                    Name = "Test Event",
                    OrganisatorId = johnDiverId,
                    Organisator = CreateDiverJohn(sendOwnNotifications),
                }
            };
            var notificationRepo = CreateNotificationRepo();
            var notificationService = CreateNotificationService(notificationRepo, sendOwnNotifications);
            
            // Act
            await notificationService.NotifyForChangedParticipationAsync(participant);

            // Assert
            A.CallTo(() => notificationRepo.InsertAsync(A<Notification>._)).MustHaveHappened(Repeated.Exactly.Times(expectedNotificationInserts));
        }

        private INotificationRepository CreateNotificationRepo() => A.Fake<INotificationRepository>();

        private NotificationService CreateNotificationService(INotificationRepository notificationRepo, bool sendOwnNotifications = false)
        {
            var diverRepo = CreateDiverRepoFakeWithTauchbolde(sendOwnNotifications);
            var participantRepo = A.Fake<IParticipantRepository>();

            return new NotificationService(
                notificationRepo,
                diverRepo,
                participantRepo
            );
        }

        private IDiverRepository CreateDiverRepoFakeWithTauchbolde(bool sendOwnNotifications = false)
        {
            var diverRepo = A.Fake<IDiverRepository>();

            A.CallTo(() => diverRepo.GetAllTauchboldeUsersAsync(false)).ReturnsLazily((call) => Task.FromResult<ICollection<Diver>>(
                new[] {
                    CreateDiverJohn(sendOwnNotifications),
                    CreateDiverJane(sendOwnNotifications),
                }
            ));

            return diverRepo;
        }

        private Diver CreateCurrentUser(bool sendOwnNotifications = false) => CreateDiverJohn(sendOwnNotifications);

        private Diver CreateDiverJohn(bool sendOwnNotifications = false) => CreateDiver(johnDiverId, "John", "Doe", sendOwnNotifications);

        private Diver CreateDiverJane(bool sendOwnNotifications = false) => CreateDiver(janeDiverId, "Jane", "Doe", sendOwnNotifications);

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
