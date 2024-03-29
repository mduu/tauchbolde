using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Application.UseCases.Logbook.NewUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class NewLogbookEntryInteractorTests
    {
        private readonly NewLogbookEntryInteractor interactor;
        private readonly ICurrentUser currentUser = A.Fake<ICurrentUser>();

        public NewLogbookEntryInteractorTests()
        {
            var logger = A.Fake<ILogger<NewLogbookEntryInteractor>>();
            var logbookEntryRepository = A.Fake<ILogbookEntryRepository>();
            var photoService = A.Fake<IPhotoService>();
            interactor = new NewLogbookEntryInteractor(logger, logbookEntryRepository, photoService, currentUser);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var request = CreateNewLogbookEntryRequest();
            
            var result = await interactor.Handle(request, CancellationToken.None);

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_SuccessWithPhoto()
        {
            var imageData = new MemoryStream(new byte[] { 13, 21, 42 });
            var request = new NewLogbookEntry(
                "The Title",
                "The Teaser",
                "The Text",
                false,
                imageData,
                "hello.jpg",
                "image/jpeg",
                null, 
                null);
            
            var result = await interactor.Handle(request, CancellationToken.None);

            result.IsSuccessful.Should().BeTrue();
        }

        private static NewLogbookEntry CreateNewLogbookEntryRequest()
        {
            var request = new NewLogbookEntry(
                "The Title",
                "The teaser",
                "The Text",
                false,
                null,
                null,
                null,
                null, null);
            return request;
        }
    }
}