using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.UseCases.Logbook.GetDetailsUseCase;
using Tauchbolde.Domain.Entities;
using Tauchbolde.SharedKernel;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class GetLogbookEntryDetailsInteractorTests
    {
        private readonly Guid validId = new Guid("544E7E71-67F8-4676-9247-BA51ACC7884A");
        private readonly ILogbookEntryRepository repository = A.Fake<ILogbookEntryRepository>();

        private readonly ILogger<GetLogbookEntryDetailsInteractor> logger =
            A.Fake<ILogger<GetLogbookEntryDetailsInteractor>>();

        private readonly GetLogbookEntryDetailsInteractor interactor;

        public GetLogbookEntryDetailsInteractorTests()
        {
            A.CallTo(() => repository.FindByIdAsync(A<Guid>._))
                .ReturnsLazily(call => Task.FromResult(
                    (Guid) call.Arguments[0] == validId
                        ? new LogbookEntry
                            {
                                Id = validId,
                                Title = "Test"
                            }
                        : null));

            interactor = new GetLogbookEntryDetailsInteractor(logger, repository);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var request = new GetLogbookEntryDetails(validId);

            var result = await interactor.Handle(request, CancellationToken.None);

            result.Payload.Should().NotBeNull();
            result.IsSuccessful.Should().BeTrue();
            result.Payload.Id.Should().Be(validId);
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_NotFound()
        {
            var request = new GetLogbookEntryDetails(new Guid("D6401A2A-1C89-4CFD-8436-53D071B1673C"));

            var result = await interactor.Handle(request, CancellationToken.None);

            result.Payload.Should().BeNull();
            result.IsSuccessful.Should().BeFalse();
            result.ResultCategory.Should().Be(ResultCategory.NotFound);
            result.Errors.Should().HaveCount(1);
        }
    }
}