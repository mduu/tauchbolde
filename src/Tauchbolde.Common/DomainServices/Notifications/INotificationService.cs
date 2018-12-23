using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Notifications
{
    /// <summary>
    /// Allows the app to register notification to be send later on.
    /// </summary>
    public interface INotificationService
    {
        Task NotifyForNewEventAsync(Event newEvent);
        Task NotifyForChangedEventAsync(Event changedEvent);
        Task NotifyForCanceledEventAsync(Event canceledEvent);
        Task NotifyForChangedParticipationAsync(Participant participant);
        Task NotifyForEventCommentAsync(Comment comment);
    }
}
