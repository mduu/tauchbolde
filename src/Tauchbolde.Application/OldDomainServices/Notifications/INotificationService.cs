using System;
using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Notifications
{
    /// <summary>
    /// Allows the app to register notification to be send later on.
    /// </summary>
    public interface INotificationService
    {
        Task NotifyForNewEventAsync(Event newEvent, Diver currentUser);
        Task NotifyForChangedEventAsync(Event changedEvent, Diver currentUser);
        Task NotifyForChangedParticipationAsync(Participant participant, Diver participatingDiver, Guid eventId);
    }
}
