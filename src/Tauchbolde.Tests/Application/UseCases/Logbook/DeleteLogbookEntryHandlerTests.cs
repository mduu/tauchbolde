using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Logbook.DeleteUseCase;
using Tauchbolde.Domain.Entities;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class DeleteLogbookEntryHandlerTests
    {
        private readonly Guid validLogbookEntryId = new Guid("C77B50BF-614B-4CC7-B611-3A240E0D5D80");
        private readonly ILogger<DeleteLogbookEntryHandler> logger = A.Fake<ILogger<DeleteLogbookEntryHandler>>();
        private readonly ILogbookEntryRepository repository = A.Fake<ILogbookEntryRepository>();
        private readonly DeleteLogbookEntryHandler handler;

        public DeleteLogbookEntryHandlerTests()
        {
            A.CallTo(() => repository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid)call.Arguments[0] == validLogbookEntryId
                        ? CreateValidLogbookEntry()
                        : null));
            
            handler = new DeleteLogbookEntryHandler(logger, repository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var request = new DeleteLogbookEntry(validLogbookEntryId);
            
            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_NonExistingId()
        {
            var request = new DeleteLogbookEntry(new Guid("5988D38B-AB1E-4EA4-BFC3-593605553A62"));
            
            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(DeleteLogbookEntry.LogbookEntryId));
        }
        
        private LogbookEntry CreateValidLogbookEntry() =>
            new LogbookEntry
            {
                Id = validLogbookEntryId,
                CreatedAt = new DateTime(2019, 1, 1),
                Title = "Test Title",
                TeaserText = "Test Teaser",
                Text = "Test Text",
            };
    }
}