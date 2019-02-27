using System;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.DomainServices.Repositories;
using Tauchbolde.Common.DomainServices.Notifications;
using Tauchbolde.Common.Infrastructure.Telemetry;
using System.Collections.Generic;

namespace Tauchbolde.Common.DomainServices.Events
{
    internal class ParticipationService : IParticipationService
    {
        private readonly ApplicationDbContext context;
        private readonly IParticipantRepository participantRepository;
        private readonly INotificationService notificationService;
        private readonly ITelemetryService telemetryService;

        public ParticipationService(
            ApplicationDbContext context,
            IParticipantRepository participantRepository,
            INotificationService notificationService,
            ITelemetryService telemetryService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            this.telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));
        }

        /// <inheritdoc />
        public async Task<Participant> GetExistingParticipationAsync(Diver user, Guid eventId)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (eventId == Guid.Empty) throw new ArgumentNullException(nameof(eventId));

            return await participantRepository.GetParticipationForEventAndUserAsync(user, eventId);
        }

        /// <inheritdoc />
        public async Task<Participant> ChangeParticipationAsync(
            Diver user,
            Guid eventId,
            ParticipantStatus status,
            int numberOfPeople,
            string note,
            string buddyTeamName)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (eventId == Guid.Empty) { throw new ArgumentNullException(nameof(eventId)); }
            if (numberOfPeople < 0) { throw new ArgumentOutOfRangeException(nameof(numberOfPeople)); }

            var participant = await participantRepository.GetParticipationForEventAndUserAsync(user, eventId);
            if (participant == null)
            {
                participant = new Participant
                {
                    Id = Guid.NewGuid(),
                    EventId = eventId,
                };

                await context.Participants.AddAsync(participant);
            }

            participant.Status = status;
            participant.ParticipatingDiver = user;
            participant.BuddyTeamName = buddyTeamName;
            participant.Note = note;
            participant.CountPeople = numberOfPeople;
            await context.SaveChangesAsync();

            var reReadParticipant = await participantRepository.FindByIdAsync(participant.Id);
            await notificationService.NotifyForChangedParticipationAsync(reReadParticipant);
            await context.SaveChangesAsync();
            TrackEvent("CHANGE-PARTICIPATION", participant);

            return participant;
        }
        
        private void TrackEvent(string name, Participant participantToTrack)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (participantToTrack == null) throw new ArgumentNullException(nameof(participantToTrack));

            telemetryService.TrackEvent(
                name,
                new Dictionary<string, string>
                {
                    { "ParticipantId", participantToTrack.Id.ToString("B") },
                    { "ParticipatingDiverId", participantToTrack.ParticipatingDiver.Id.ToString("B")},
                    { "Status", participantToTrack.Status.ToString() },
                    { "CountPeople", participantToTrack.CountPeople.ToString() },
                    { "Note", participantToTrack.Note },
                });
        }
    }
}
