using FakeItEasy;
using Tauchbolde.Application.Policies.Logbook.LogbookEntryDeleted;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Domain.Events.LogbookEntry;
using Tauchbolde.Domain.ValueObjects;
using Xunit;

namespace Tauchbolde.Tests.Application.Policies.Logbook
{
    public class DeleteLogbookTeaserPhotoPolicyTests
    {
        private readonly Guid validLogbookEntryId = new Guid("FE4272CC-E0DA-479F-84D0-B7DF3810FCB9");
        private readonly IPhotoService photoService = A.Fake<IPhotoService>();
        private readonly DeleteLogbookTeaserPhotoPolicy policy;

        public DeleteLogbookTeaserPhotoPolicyTests()
        {
            policy = new DeleteLogbookTeaserPhotoPolicy(photoService);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var notification = new LogbookEntryDeletedEvent(
                validLogbookEntryId, 
                "LogbookTeaser/test.jpg", 
                "LogbookTeaser/thumbs/test.jpg");

            await policy.Handle(notification, CancellationToken.None);

            A.CallTo(() => photoService.RemovePhotosAsync(A<PhotoIdentifier[]>._))
                .MustHaveHappenedOnceExactly();
        }
    }
}