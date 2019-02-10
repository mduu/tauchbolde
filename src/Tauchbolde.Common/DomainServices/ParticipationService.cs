using System;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Common.DomainServices.Notifications;

namespace Tauchbolde.Common.DomainServices
{
    public class ParticipationService : IParticipationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IParticipantRepository _participantRepository;
        private readonly INotificationService notificationService;

        public ParticipationService(
            ApplicationDbContext context,
            IParticipantRepository participantRepository,
            INotificationService notificationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        /// <inheritdoc />
        public async Task<Participant> GetExistingParticipationAsync(Diver user, Guid eventId)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (eventId == Guid.Empty) throw new ArgumentNullException(nameof(eventId));

            return await _participantRepository.GetParticipationForEventAndUserAsync(user, eventId);
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

            var participant = await _participantRepository.GetParticipationForEventAndUserAsync(user, eventId);
            if (participant == null)
            {
                participant = new Participant
                {
                    Id = Guid.NewGuid(),
                    EventId = eventId,
                };

                await _context.Participants.AddAsync(participant);
            }

            participant.Status = status;
            participant.ParticipatingDiver = user;
            participant.BuddyTeamName = buddyTeamName;
            participant.Note = note;
            participant.CountPeople = numberOfPeople;
            await _context.SaveChangesAsync();

            var reReadParticipant = await _participantRepository.FindByIdAsync(participant.Id);
            await notificationService.NotifyForChangedParticipationAsync(reReadParticipant);
            await _context.SaveChangesAsync();

            return participant;
        }
    }
}
