using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Logbook.UnpublishUseCase;
using Tauchbolde.Domain.Entities;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class UnpublishLogbookEntryInteractorTests
    {
        private readonly Guid validLogbookEntryId = new Guid("EA07BF09-C77E-4198-B180-A1FCD43CDEAC");
        private readonly UnpublishLogbookEntryInteractor interactor;
        private readonly ILogbookEntryRepository repository;

        public UnpublishLogbookEntryInteractorTests()
        {
            var logger = A.Fake<ILogger<UnpublishLogbookEntryInteractor>>();
            
            repository = A.Fake<ILogbookEntryRepository>();
            A.CallTo(() => repository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid)call.Arguments[0] == validLogbookEntryId
                        ? new LogbookEntry { Id = validLogbookEntryId, Title = "TheTitle" }
                        : null
                ));
            
            interactor = new UnpublishLogbookEntryInteractor(logger, repository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            UnpublishLogbookEntry request = new UnpublishLogbookEntry(validLogbookEntryId);
            
            var result = await interactor.Handle(request, CancellationToken.None);

            result.Should().BeTrue();
            A.CallTo(() => repository.UpdateAsync(
                    A<LogbookEntry>.That.Matches(l => 
                        l.Id == validLogbookEntryId && !l.IsPublished)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Handle_NotFound()
        {
            UnpublishLogbookEntry request = new UnpublishLogbookEntry(new Guid("8A2BD418-1936-4BCD-8AC5-A1732DD0998A"));
            
            var result = await interactor.Handle(request, CancellationToken.None);

            result.Should().BeFalse();
            A.CallTo(() => repository.UpdateAsync(A<LogbookEntry>.That.Matches(l => l.Id == validLogbookEntryId)))
                .MustNotHaveHappened();
        }
    }
}