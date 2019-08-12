using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.Services.Notifications
{
    /// <summary>
    /// Methods to build collections of recipients.
    /// </summary>
    internal interface IRecipientsBuilder
    {
        Task<List<Diver>> GetAllTauchboldeButDeclinedParticipantsAsync(
            Guid currentDiverId,
            Guid eventId);
    }
}