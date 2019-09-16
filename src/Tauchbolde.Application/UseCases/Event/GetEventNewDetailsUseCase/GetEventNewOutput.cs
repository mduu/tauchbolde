using System;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Event.GetEventNewDetailsUseCase
{
    public class GetEventNewOutput
    {
        public GetEventNewOutput(
            [NotNull] string organizatorName,
            [NotNull] string organizatorEmail,
            [CanBeNull] string organizatorAvatarId,
            DateTime startTime,
            DateTime? endTime)
        {
            OrganizatorName = organizatorName ?? throw new ArgumentNullException(nameof(organizatorName));
            OrganizatorEmail = organizatorEmail ?? throw new ArgumentNullException(nameof(organizatorEmail));
            OrganizatorAvatarId = organizatorAvatarId;
            StartTime = startTime;
            EndTime = endTime;
        }

        [NotNull] public string OrganizatorName { get; }
        [NotNull] public string OrganizatorEmail { get; }
        [CanBeNull] public string OrganizatorAvatarId { get; }
        public DateTime StartTime { get; }
        public DateTime? EndTime { get; }
    }
}