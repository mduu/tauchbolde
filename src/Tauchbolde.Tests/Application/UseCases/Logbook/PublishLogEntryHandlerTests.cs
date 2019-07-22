using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Logbook.PublishUseCase;
using Tauchbolde.Domain.Entities;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class PublishLogEntryHandlerTests
    {
        private static readonly Guid ValidLogbookEntryId = new Guid("0D8C32AD-0C6B-4A39-A054-456ECE524F2A");
        private readonly PublishLogbookEntryHandler publishLogbookEntryHandler;
        private readonly ILogbookEntryRepository dataAccess;
        private readonly PublishLogbookEntry request = new PublishLogbookEntry(ValidLogbookEntryId);

        public PublishLogEntryHandlerTests()
        {
            var logger = A.Fake<ILogger<PublishLogbookEntryHandler>>();
            dataAccess = A.Fake<ILogbookEntryRepository>();
            publishLogbookEntryHandler = new PublishLogbookEntryHandler(logger, dataAccess);
        }

        [Fact]
        public async Task Handle_Success()
        {
            A.CallTo(() => dataAccess.FindByIdAsync(ValidLogbookEntryId))
                .ReturnsLazily(() =>
                    Task.FromResult(new LogbookEntry {Id = ValidLogbookEntryId}));
            
            var result = await publishLogbookEntryHandler.Handle(request, new CancellationToken());

            result.Should().BeTrue();
            A.CallTo(() => dataAccess.UpdateAsync(
                    A<LogbookEntry>.That.Matches(l => l.Id == ValidLogbookEntryId)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_LogbookEntryNotFound()
        {
            A.CallTo(() => dataAccess.FindByIdAsync(ValidLogbookEntryId))
                .ReturnsLazily(() =>
                    Task.FromResult<LogbookEntry>(null));
            
            var result = await publishLogbookEntryHandler.Handle(request, new CancellationToken());

            result.Should().BeFalse();
            A.CallTo(() => dataAccess.UpdateAsync(
                    A<LogbookEntry>.That.Matches(l => l.Id == ValidLogbookEntryId)))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task Handle_ErrorDuringUpdate()
        {
            A.CallTo(() => dataAccess.FindByIdAsync(ValidLogbookEntryId))
                .ReturnsLazily(() =>
                    Task.FromResult(new LogbookEntry {Id = ValidLogbookEntryId}));
            A.CallTo(() => dataAccess.UpdateAsync(A<LogbookEntry>._))
                .Throws<InvalidOperationException>();

            var result = await publishLogbookEntryHandler.Handle(request, new CancellationToken());
                
            result.Should().BeFalse();
            A.CallTo(() => dataAccess.UpdateAsync(
                    A<LogbookEntry>.That.Matches(l => l.Id == ValidLogbookEntryId)))
                .MustHaveHappenedOnceExactly();
        }
    }
}