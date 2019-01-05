using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    /// <summary>
    /// Allows the app to register notification to be send later on.
    /// </summary>
    public interface INotificationService
    {
        Task NotifyForNewEventAsync(Event newEvent, Diver currentUser);
        Task NotifyForChangedEventAsync(Event changedEvent, Diver currentUser);
        Task NotifyForCanceledEventAsync(Event canceledEvent, Diver currentUser);
        Task NotifyForChangedParticipationAsync(Participant participant);
        Task NotifyForEventCommentAsync(Comment comment, Diver author);
    }
}
