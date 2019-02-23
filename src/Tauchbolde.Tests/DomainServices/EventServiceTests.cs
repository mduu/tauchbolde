﻿using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Namers;
using FakeItEasy;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.DomainServices.Events;
using Tauchbolde.Common.DataAccess;
using Tauchbolde.Common.DomainServices.Notifications;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using Tauchbolde.Common.Infrastructure.Telemetry;

namespace Tauchbolde.Tests.DomainServices
{
    public class EventServiceTests
    {
        private Guid eventId = new Guid("e6a5f186-31f0-4424-9fd3-89d6935e19eb");
        private Guid currentDiverId = new Guid("09a578b2-0bc9-4bc5-90bc-ee63cda48cc6");
        private INotificationService notitifacationService;
        private readonly Diver currentDiver;

        public EventServiceTests()
        {
            currentDiver = new Diver
            {
                Id = currentDiverId,
                Fullname = "John Doe",
                Firstname = "John",
                Lastname = "Doe"
            };
        }

        [Theory]
        [UseReporter(typeof(DiffReporter))]
        [InlineData("StartEnd", "2018/12/13 19:00:00", "2018/12/13 23:00:00")]
        [InlineData("Start", "2018/12/13 19:00:00", null)]
        [InlineData("StartEndMultiDay", "2018/12/13 19:00:00", "2018/12/15 23:00:00")]
        public async Task CreateIcalForEventAsync(string name, string startTime, string endTime)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfoByIetfLanguageTag("en-US");
            using (ApprovalResults.ForScenario(name))
            {
                // ARRANGE
                var start = DateTimeOffset.ParseExact(startTime, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                var end = endTime != null
                    ? (DateTimeOffset?)DateTimeOffset.ParseExact(endTime, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
                    : null;

                var evt = CreateEvent(start, end);
                var eventService = CreateEventService(evt);
                var createDateTime = new DateTime(2018, 9, 13, 8, 0, 0);

                // ACT
                var icalStream = await eventService.CreateIcalForEventAsync(eventId, createDateTime);

                // ASSERT
                var reader = new StreamReader(icalStream);
                var icalText = reader.ReadToEnd();

                Approvals.Verify(icalText);
            }
        }

        [Fact]
        public async Task NewEvent()
        {
            // ARRANGE
            var eventService = CreateEventService(null);
            var newEvt = CreateEvent(
                new DateTimeOffset(2018, 09, 23, 18, 00, 0, TimeSpan.Zero),
                new DateTimeOffset(2018, 09, 23, 22, 00, 0, TimeSpan.Zero),
                Guid.Empty);
            
            // ACT
            var createdEvent = await eventService.UpsertEventAsync(newEvt, currentDiver);
            newEvt.Id = createdEvent.Id;

            // ASSERT
            createdEvent.Should().BeEquivalentTo(newEvt);
            A.CallTo(() => notitifacationService.NotifyForNewEventAsync(createdEvent, currentDiver)).MustHaveHappenedOnceExactly();
            A.CallTo(() => notitifacationService.NotifyForChangedEventAsync(A<Event>._, currentDiver)).MustNotHaveHappened();
        }
        
        [Fact]
        public async Task UpdateEvent()
        {
            // ARRANGE
            var evt = CreateEvent(
                new DateTimeOffset(2018, 09, 23, 19, 00, 0, TimeSpan.Zero),
                new DateTimeOffset(2018, 09, 23, 23, 00, 0, TimeSpan.Zero));
            var eventService = CreateEventService(evt);
            var updateEvt = CreateEvent(
                new DateTimeOffset(2018, 09, 23, 19, 30, 0, TimeSpan.Zero),
                new DateTimeOffset(2018, 09, 23, 23, 30, 0, TimeSpan.Zero));
            

            // ACT
            var updatedEvent = await eventService.UpsertEventAsync(updateEvt, currentDiver);

            // ASSERT
            updatedEvent.Should().BeEquivalentTo(updateEvt);
            A.CallTo(() => notitifacationService.NotifyForChangedEventAsync(updatedEvent, currentDiver)).MustHaveHappenedOnceExactly();
            A.CallTo(() => notitifacationService.NotifyForNewEventAsync(A<Event>._, currentDiver)).MustNotHaveHappened();
        }

        private EventService CreateEventService(Event evt)
        {
            return new EventService(
                A.Fake<ApplicationDbContext>(),
                CreateNotificationService(),
                CreateEventRepositoryFake(evt),
                A.Fake<ICommentRepository>(),
                A.Fake<ITelemetryService>());
        }

        private Event CreateEvent(DateTimeOffset start, DateTimeOffset? end, Guid? id = null)
        {
            return new Event
            {
                Id = id ?? eventId,
                StartTime = start.ToUniversalTime().DateTime,
                EndTime = end?.ToUniversalTime().DateTime,
                Name = "Test Event",
                Description = "Description Test",
                Location = "Lake 1",
                MeetingPoint = "MP Test",
            };
        }

        private IEventRepository CreateEventRepositoryFake(Event evt)
        {
            var fake = A.Fake<IEventRepository>();
            A.CallTo(() => fake.FindByIdAsync(eventId)).ReturnsLazily(
               () => Task.FromResult(evt));

            return fake;
        }

        private INotificationService CreateNotificationService()
        {
            notitifacationService = A.Fake<INotificationService>();

            return notitifacationService;
        }
    }
}
