using System;
using System.Collections.Generic;
using System.Text;
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
        public async Task ChangeParticipationAsync(ApplicationUser user, Guid? existingParticipantId, Guid eventId,
            ParticipantStatus status, int numberOfPeople, string note, string buddyTeamName)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (eventId == null) { throw new ArgumentNullException(nameof(eventId)); }
            if (numberOfPeople < 0) { throw new ArgumentOutOfRangeException(nameof(numberOfPeople)); }

            Participant participant;
            if (existingParticipantId.HasValue)
            {
                participant = await _participantRepository.FindByIdAsync(existingParticipantId.Value);
            }
            else
            {
                participant = new Participant { Id = Guid.NewGuid() };
                await _context.Participants.AddAsync(participant);
            }

            participant.EventId = eventId;
            participant.Status = status;
            participant.User = user;
            participant.BuddyTeamName = buddyTeamName;
            participant.Note = note;
            participant.CountPeople = numberOfPeople;

            await _context.SaveChangesAsync();

        }
    }
}
