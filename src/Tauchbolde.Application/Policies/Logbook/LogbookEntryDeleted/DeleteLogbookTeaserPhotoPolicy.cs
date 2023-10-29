using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Domain.Events.LogbookEntry;
using Tauchbolde.Domain.ValueObjects;

namespace Tauchbolde.Application.Policies.Logbook.LogbookEntryDeleted
{
    [UsedImplicitly]
    public class DeleteLogbookTeaserPhotoPolicy : INotificationHandler<LogbookEntryDeletedEvent>
    {
        private readonly IPhotoService photoService;

        public DeleteLogbookTeaserPhotoPolicy([NotNull] IPhotoService photoService)
        {
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }
        
        public async Task Handle([NotNull] LogbookEntryDeletedEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            
            var identifiersToDelete = new List<PhotoIdentifier>();

            if (!string.IsNullOrWhiteSpace(notification.TeaserImage))
            {
                identifiersToDelete.Add(new PhotoIdentifier(notification.TeaserImage));
            }

            if (!string.IsNullOrWhiteSpace(notification.TeaserImageThumb))
            {
                identifiersToDelete.Add(new PhotoIdentifier(notification.TeaserImageThumb));
            }

            await photoService.RemovePhotosAsync(identifiersToDelete.ToArray());

        }
    }
}