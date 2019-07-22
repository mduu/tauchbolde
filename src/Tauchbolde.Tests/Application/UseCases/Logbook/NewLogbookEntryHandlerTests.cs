using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Application.UseCases.Logbook.NewUseCase;
using Xunit;

namespace Tauchbolde.Tests.Application.UseCases.Logbook
{
    public class NewLogbookEntryHandlerTests
    {
        private readonly NewLogbookEntryHandler handler;
        private readonly ILogbookEntryRepository logbookEntryRepository;
        private readonly IPhotoService service;

        public NewLogbookEntryHandlerTests()
        {
            var logger = A.Fake<ILogger<NewLogbookEntryHandler>>();
            logbookEntryRepository = A.Fake<ILogbookEntryRepository>();
            service = A.Fake<IPhotoService>();
            handler = new NewLogbookEntryHandler(logger, logbookEntryRepository, service);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var request = CreateNewLogbookEntryRequest();
            
            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_SuccessWithPhoto()
        {
            var imageData = new MemoryStream(new byte[] { 13, 21, 42 });
            var request = new NewLogbookEntry(
                new Guid("94441135-BAD0-4EAD-AE32-0DD72EC2D159"),
                "The Title",
                "The Text",
                null,
                false,
                imageData,
                "hello.jpg",
                "image/jpeg",
                null,
                null);
            
            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.Should().BeTrue();
        }

        private static NewLogbookEntry CreateNewLogbookEntryRequest()
        {
            var request = new NewLogbookEntry(
                new Guid("94441135-BAD0-4EAD-AE32-0DD72EC2D159"),
                "The Title",
                "The Text",
                null,
                false,
                null,
                null,
                null,
                null,
                null);
            return request;
        }
    }
}