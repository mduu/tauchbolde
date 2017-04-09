using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices
{
    public interface INotificationService
    {
        Task NotifyForNewEventAsync(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, Event newEvent);
        Task NotifyForChangedEventAsync(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, Event changedEvent);
        Task NotifyForCanceledEventAsync(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, IParticipantRepository participantRepository, Event canceledEvent);
        Task NotifyForChangedParticipation(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, IParticipantRepository participantRepository, Participant participant);
        Task NotifyForEventComment(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, Comment comment);
        Task NotifyForNewPost(INotificationRepository notificationRepository, IApplicationUserRepository userRepository, Post newPost);
    }
}
