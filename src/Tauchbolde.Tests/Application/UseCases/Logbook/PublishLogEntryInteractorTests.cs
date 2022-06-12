using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Logbook.PublishUseCase;
using Tauchbolde.Domain.Entities;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class PublishLogEntryInteractorTests
    {
        private static readonly Guid ValidLogbookEntryId = new Guid("0D8C32AD-0C6B-4A39-A054-456ECE524F2A");
        private readonly PublishLogbookEntryInteractor publishLogbookEntryInteractor;
        private readonly ILogbookEntryRepository dataAccess;
        private readonly PublishLogbookEntry request = new PublishLogbookEntry(ValidLogbookEntryId);

        public PublishLogEntryInteractorTests()
        {
            var logger = A.Fake<ILogger<PublishLogbookEntryInteractor>>();
            dataAccess = A.Fake<ILogbookEntryRepository>();
            publishLogbookEntryInteractor = new PublishLogbookEntryInteractor(logger, dataAccess);
        }

        [Fact]
        public async Task Handle_Success()
        {
            A.CallTo(() => dataAccess.FindByIdAsync(ValidLogbookEntryId))
                .ReturnsLazily(() =>
                    Task.FromResult(new LogbookEntry {Id = ValidLogbookEntryId}));
            
            var result = await publishLogbookEntryInteractor.Handle(request, new CancellationToken());

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
            
            var result = await publishLogbookEntryInteractor.Handle(request, new CancellationToken());

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

            var result = await publishLogbookEntryInteractor.Handle(request, new CancellationToken());
                
            result.Should().BeFalse();
            A.CallTo(() => dataAccess.UpdateAsync(
                    A<LogbookEntry>.That.Matches(l => l.Id == ValidLogbookEntryId)))
                .MustHaveHappenedOnceExactly();
        }
    }
}