using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Application.Services.Notifications
{
    internal class RecipientsBuilder : IRecipientsBuilder
    {
        private readonly IParticipantRepository participantRepository;
        private readonly IDiverRepository diverRepository;

        public RecipientsBuilder([NotNull] IParticipantRepository participantRepository, [NotNull] IDiverRepository diverRepository)
        {
            this.participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
        }

        public async Task<List<Diver>> GetAllTauchboldeButDeclinedParticipantsAsync(
            Guid currentDiverId,
            Guid eventId)
        {
            var declinedParticipants =
                (await participantRepository.GetParticipantsForEventByStatusAsync(eventId, ParticipantStatus.Declined))
                .Where(p => p.ParticipatingDiver.Id != currentDiverId);

            var result = (await diverRepository
                    .GetAllTauchboldeUsersAsync())
                .Where(u =>
                    declinedParticipants.All(p => p.ParticipatingDiver.Id != u.Id))
                .ToList();

            return result;
        }

    }
}