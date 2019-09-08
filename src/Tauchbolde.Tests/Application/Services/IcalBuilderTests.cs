using System;
using System.Globalization;
using System.IO;
using System.Threading;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using Tauchbolde.Application.Services;
using Xunit;

namespace Tauchbolde.Tests.Application.Services
{
    [UseReporter(typeof(DiffReporter))]
    public class IcalBuilderTests
    {
        private readonly Guid eventId = new Guid("e6a5f186-31f0-4424-9fd3-89d6935e19eb");
        private readonly IcalBuilder builder;

        public IcalBuilderTests()
        {
            builder = new IcalBuilder()
                .Id(eventId)
                .Title("Test Event")
                .Description("Description Test")
                .Location("Lake 1")
                .MeetingPoint("MP Test")
                .CreateTime(new DateTime(2018, 9, 13, 8, 0, 0));
        }
        
        [Theory]
        [InlineData("StartEnd", "2018/12/13 19:00:00", "2018/12/13 23:00:00")]
        [InlineData("Start", "2018/12/13 19:00:00", null)]
        [InlineData("StartEndMultiDay", "2018/12/13 19:00:00", "2018/12/15 23:00:00")]
        public void Test(string name, string startTime, string endTime)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfoByIetfLanguageTag("en-US");
            using (ApprovalResults.ForScenario(name))
            {
                // Arrange
                var start = DateTimeOffset.ParseExact(startTime, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                var end = endTime != null
                    ? (DateTimeOffset?)DateTimeOffset.ParseExact(endTime, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
                    : null;
                builder
                    .StartTime(start.ToUniversalTime().DateTime)
                    .EndTime(end?.ToUniversalTime().DateTime);

                // Act
                var icalStream = builder.Build();

                // Assert
                var reader = new StreamReader(icalStream);
                var icalText = reader.ReadToEnd();
                Approvals.Verify(icalText);
            }
        }
    }
}