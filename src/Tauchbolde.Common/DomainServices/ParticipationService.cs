using System;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices
{
    public class ParticipationService : IParticipationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IParticipantRepository _participantRepository;

        public ParticipationService(
            ApplicationDbContext context,
            IParticipantRepository participantRepository)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (participantRepository == null) { throw new ArgumentNullException(nameof(participantRepository)); }

            _context = context;
            _participantRepository = participantRepository;
        }

        /// <inheritdoc />
        public async Task<Participant> GetExistingParticipationAsync(ApplicationUser user, Guid eventId)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (eventId == null) throw new ArgumentNullException(nameof(eventId));

            return await _participantRepository.GetParticipationForEventAndUserAsync(user, eventId);
        }

        /// <inheritdoc />
        public async Task<Participant> ChangeParticipationAsync(ApplicationUser user, Guid eventId, ParticipantStatus status, int numberOfPeople, string note, string buddyTeamName)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (eventId == null) { throw new ArgumentNullException(nameof(eventId)); }
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
            participant.User = user;
            participant.BuddyTeamName = buddyTeamName;
            participant.Note = note;
            participant.CountPeople = numberOfPeople;

            await _context.SaveChangesAsync();

            return participant;
        }
    }
}
