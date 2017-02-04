using System;
using System.Collections.Generic;
using System.Text;

namespace Tauchbolde.Common.Model
{
    public class Notifications
    {
        public Guid Id { get; set; }
        public ApplicationUser Recipient { get; set; }
        public DateTime OccuredAt { get; set; }
        public bool AlreadySent { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
    }
}
