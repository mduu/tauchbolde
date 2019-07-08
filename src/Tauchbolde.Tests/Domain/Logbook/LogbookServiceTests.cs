using System;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tauchbolde.Common.Domain.Logbook;
using Tauchbolde.Common.Domain.Notifications;
using Tauchbolde.Common.Domain.PhotoStorage;
using Tauchbolde.Common.Domain.Repositories;
using Tauchbolde.Common.Domain.Users;
using Tauchbolde.Common.Infrastructure.Telemetry;
using Tauchbolde.Common.Model;
using Xunit;

namespace Tauchbolde.Tests.Domain.Logbook
{
    public class LogbookServiceTests
    {
        private readonly ILogbookEntryRepository logbookEntryRepoFake;
        private readonly LogbookService logbookService;
        private readonly LogbookUpsertModel validInsertModel;
        private readonly LogbookUpsertModel validUpdateModel;

        public LogbookServiceTests()
        {
            logbookEntryRepoFake = A.Fake<ILogbookEntryRepository>();
            var diverServiceFake = A.Fake<IDiverService>();
            var telemetryServiceFake = A.Fake<ITelemetryService>();
            var photoService = A.Fake<IPhotoService>();

            logbookService = new LogbookService(
                logbookEntryRepoFake,
                diverServiceFake,
                telemetryServiceFake,
                photoService,
                A.Fake<ILogger<LogbookService>>(),
                A.Fake<INotificationService>());

            validInsertModel = CreateValidModel(null);
            validUpdateModel = CreateValidModel(new Guid("50D106BD-D47A-47A5-8244-CF3560A2A3E4"));
        }

        [Fact]
        public async Task TestInsertSuccess()
        {
            var id = await logbookService.UpsertAsync(validInsertModel);

            id.Should().NotBeEmpty();
            A.CallTo(() => logbookEntryRepoFake.InsertAsync(A<LogbookEntry>._)).MustHaveHappened();
        }

        [Fact]
        public void TestInsertWithoutTitle()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            validInsertModel.Title = null;

            Func<Task> act = async () => await logbookService.UpsertAsync(validInsertModel);

            act.Should().Throw<InvalidOperationException>().WithMessage("Title must not be null or empty!");
        }
        
        [Fact]
        public void TestInsertWithoutText()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            validInsertModel.Text = null;

            Func<Task> act = async () => await logbookService.UpsertAsync(validInsertModel);

            act.Should().Throw<InvalidOperationException>().WithMessage("Text must not be null or empty!");
        }

        [Fact]
        public async Task TestUpdateSuccess()
        {
            A.CallTo(() => logbookEntryRepoFake.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call =>
                    Task.FromResult(
                        new LogbookEntry
                        {
                            Id = (Guid) call.Arguments[0],
                            Title = "Hi",
                        }
                    )
                );

            var id = await logbookService.UpsertAsync(validUpdateModel);

            id.Should().NotBeEmpty();
            A.CallTo(() => logbookEntryRepoFake.Update(A<LogbookEntry>._)).MustHaveHappened();
        }
        
        [Fact]
        public void TestUpdateWithoutTitle()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            validUpdateModel.Title = null;

            Func<Task> act = async () => await logbookService.UpsertAsync(validUpdateModel);

            act.Should().Throw<InvalidOperationException>().WithMessage("Title must not be null or empty!");
        }
        
        [Fact]
        public void TestUpdateWithoutText()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            validUpdateModel.Text = null;

            Func<Task> act = async () => await logbookService.UpsertAsync(validUpdateModel);

            act.Should().Throw<InvalidOperationException>().WithMessage("Text must not be null or empty!");
        }

        private static LogbookUpsertModel CreateValidModel(Guid? id)
            => new LogbookUpsertModel
            {
                Id = id,
                Title = "This is a test",
                Text = "Here comes text.",
                Teaser = "This is a teaser",
                CreatedAt = new DateTime(2019, 03, 27, 20, 22, 00),
                CurrentDiverId = new Guid("AD8DB033-FCCC-4CEF-80B5-C2BC921E75E1"),
            };
    }
}