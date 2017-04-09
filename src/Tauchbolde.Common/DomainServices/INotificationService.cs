using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices
{
    public interface INotificationService
    {
        Task NotifyForNewEventAsync(INotificationRepository notificationRepository, Event newEvent);
        Task NotifyForChangedEventAsync(INotificationRepository notificationRepository, Event changedEvent);
        Task NotifyForChangedParticipation(INotificationRepository notificationRepository, Participant participant);
        Task NotifyForEventComment(INotificationRepository notificationRepository, Comment comment);
        Task NotifyForNewPost(INotificationRepository notificationRepository, Post newPost);
    }
}
