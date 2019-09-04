using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.InterfaceAdapters.Event.Details
{
    public class EventParticipationViewModel
    {
        public EventParticipationViewModel(
            Guid eventId,
            [NotNull] IEnumerable<string> buddyTeamNames,
            [NotNull] IEnumerable<EventParticipantViewModel> participants,
            ParticipantStatus currentUserStatus,
            string currentUserNote,
            string currentUserBuddyTeamName,
            int currentUserCountPeople)
        {
            EventId = eventId;
            BuddyTeamNames = buddyTeamNames ?? throw new ArgumentNullException(nameof(buddyTeamNames));
            Participants = participants ?? throw new ArgumentNullException(nameof(participants));
            CurrentUserStatus = currentUserStatus;
            CurrentUserNote = currentUserNote;
            CurrentUserBuddyTeamName = currentUserBuddyTeamName;
            CurrentUserCountPeople = currentUserCountPeople;
        }

        public Guid EventId { get; }
        [NotNull] public IEnumerable<string> BuddyTeamNames { get; }
        [NotNull] public IEnumerable<EventParticipantViewModel> Participants { get; }
        
        public ParticipantStatus CurrentUserStatus { get; }
        public string CurrentUserNote { get; }
        public int CurrentUserCountPeople { get; }
        public string CurrentUserBuddyTeamName { get; }
    }
}