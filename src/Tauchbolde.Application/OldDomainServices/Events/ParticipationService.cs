using System;
using System.Threading.Tasks;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.OldDomainServices.Notifications;
using Tauchbolde.Application.Services.Telemetry;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Events
{
    internal class ParticipationService : IParticipationService
    {
        private readonly IParticipantRepository participantRepository;
        private readonly INotificationService notificationService;
        private readonly ITelemetryService telemetryService;

        public ParticipationService(
            IParticipantRepository participantRepository,
            INotificationService notificationService,
            ITelemetryService telemetryService)
        {
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
    }
}
