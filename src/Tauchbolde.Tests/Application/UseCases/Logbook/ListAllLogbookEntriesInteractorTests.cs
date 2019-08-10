using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Logbook.ListAllUseCase;
using Tauchbolde.Domain.Entities;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class ListAllLogbookEntriesInteractorTests
    {
        private readonly ILogbookEntryRepository repository = A.Fake<ILogbookEntryRepository>();
        private readonly ListAllLogbookEntriesInteractor interactor;

        public ListAllLogbookEntriesInteractorTests()
        {
            A.CallTo(() => repository.GetAllEntriesAsync(false))
                .ReturnsLazily(call => Task.FromResult(new List<LogbookEntry>
                {
                    new LogbookEntry { Id = new Guid("05CFFBD4-356C-405C-9BF3-9A756722E9C8") },
                } as ICollection<LogbookEntry>));
            
            A.CallTo(() => repository.GetAllEntriesAsync(true))
                .ReturnsLazily(call => Task.FromResult(new List<LogbookEntry>
                {
                    new LogbookEntry { Id = new Guid("05CFFBD4-356C-405C-9BF3-9A756722E9C8") },
                    new LogbookEntry { Id = new Guid("12818D43-F0BF-4762-8A7E-F1023B81FEA4") },
                } as ICollection<LogbookEntry>));
            
            interactor = new ListAllLogbookEntriesInteractor(repository);
        }

        [Fact]
        public async Task Handle_DontIncludeUnpublished()
        {
            var request = new ListAllLogbookEntries(false);
            
            var result = await interactor.Handle(request, CancellationToken.None);

            result.Payload.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_IncludeUnpublished()
        {
            var request = new ListAllLogbookEntries(true);
            
            var result = await interactor.Handle(request, CancellationToken.None);

            result.Payload.Should().HaveCount(2);
        }
    }
}