using System.Threading.Tasks;
using Tauchbolde.Entities;

namespace Tauchbolde.Common.Domain.Notifications
{
    /// <summary>
    /// Allows the app to register notification to be send later on.
    /// </summary>
    public interface INotificationService
    {
        Task NotifyForNewEventAsync(Event newEvent, Diver currentUser);
        Task NotifyForChangedEventAsync(Event changedEvent, Diver currentUser);
        Task NotifyForCanceledEventAsync(Event canceledEvent, Diver currentUser);
        Task NotifyForChangedParticipationAsync(Participant participant, Diver participatingDiver, Event participatingEvent);
        Task NotifyForEventCommentAsync(Comment comment, Event evt, Diver author);
        Task NotifyForNewLogbookEntry(LogbookEntry logbookEntry, Diver author);
    }
}
