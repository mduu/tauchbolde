using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Tauchbolde.Domain.Types;
using Tauchbolde.SharedKernel;
using Tauchbolde.SharedKernel.Services;

namespace Tauchbolde.Domain.Entities
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class Notification : EntityBase
    {
        public Notification(
            Diver recipient,
            NotificationType notificationType,
            string message,
            [CanBeNull] Event relatedEvent,
            [CanBeNull] LogbookEntry logbookEntry)
        {
            Id = Guid.NewGuid();
            OccuredAt = SystemClock.Now;
            Recipient = recipient;
            Type = notificationType;
            Message = message;
            Event = relatedEvent;
            LogbookEntry = logbookEntry;
        }
        
        internal Notification()
        {
        }
        
        [Required] public Diver Recipient { get; internal set; }
        [Required] public DateTime OccuredAt { get; internal set; }
        [Required] public bool AlreadySent { get; internal set; }
        [Required] public int CountOfTries { get; internal set; }
        [Required] public string Message { get; internal set; }
        [Required] public NotificationType Type { get; internal set; }
        public Guid? EventId { get; [UsedImplicitly] internal set; }
        public virtual Event Event { get; [UsedImplicitly] internal set; }
        public Guid? LogbookEntryId { get; [UsedImplicitly] internal set; }
        public virtual LogbookEntry LogbookEntry { get; [UsedImplicitly] internal set; }

        public void TriedSending()
        {
            CountOfTries++;
        }
        
        public void Sent()
        {
            AlreadySent = true;
        }
    }
}
