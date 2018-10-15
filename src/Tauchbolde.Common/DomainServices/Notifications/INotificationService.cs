using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    /// <summary>
    /// Allows the app to register notification to be send later on.
    /// </summary>
    public interface INotificationService
    {
        Task NotifyForNewEventAsync(INotificationRepository notificationRepository, IDiverRepository userRepository, Event newEvent);
        Task NotifyForChangedEventAsync(INotificationRepository notificationRepository, IDiverRepository userRepository, Event changedEvent);
        Task NotifyForCanceledEventAsync(INotificationRepository notificationRepository, IDiverRepository userRepository, IParticipantRepository participantRepository, Event canceledEvent);
        Task NotifyForChangedParticipation(INotificationRepository notificationRepository, IDiverRepository userRepository, IParticipantRepository participantRepository, Participant participant);
        Task NotifyForEventComment(INotificationRepository notificationRepository, IDiverRepository userRepository, IParticipantRepository participantRepository, Comment comment);
    }
}
