using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Namers;
using FakeItEasy;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Common.DomainServices.Notifications;


namespace Tauchbolde.Tests.DomainServices
{
    public class EventServiceTests
    {
        private Guid eventId = new Guid("e6a5f186-31f0-4424-9fd3-89d6935e19eb");
                
        [Theory]
        [UseReporter(typeof(MsTestReporter))]
        [InlineData("StartEnd", "2018/12/13 19:00:00", "2018/12/13 23:00:00")]
        [InlineData("Start", "2018/12/13 19:00:00", null)]
        [InlineData("StartEndMultiDay", "2018/12/13 19:00:00", "2018/12/15 23:00:00")]
        public async Task CreateIcalForEventAsync(string name, string startTime, string endTime)
        {
            using (ApprovalResults.ForScenario(name))
            {
                // ARRANGE
                var start = DateTime.Parse(startTime);
                var end = endTime != null ? (DateTime?)DateTime.Parse(endTime) : null;
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

        private EventService CreateEventService(Event evt)
        {
            return new EventService(
                A.Fake<ApplicationDbContext>(),
                CreateNotificationService(),
                CreateEventRepositoryFake(evt));
        }

        private Event CreateEvent(DateTime start, DateTime? end)
        {
            return new Event
            {
                Id = eventId,
                StartTime = start,
                EndTime = end,
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
            var fake = A.Fake<INotificationService>();
            
            return fake;
        }
    }
}
