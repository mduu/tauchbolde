using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices
{
    public class NotificationService : INotificationService
    {
        public async Task NotifyForNewEventAsync(INotificationRepository notificationRepository, Event newEvent)
        {
            throw new NotImplementedException();
        }

        public async Task NotifyForChangedEventAsync(INotificationRepository notificationRepository, Event changedEvent)
        {
            throw new NotImplementedException();
        }

        public async Task NotifyForChangedParticipation(INotificationRepository notificationRepository, Participant participant)
        {
            throw new NotImplementedException();
        }

        public async Task NotifyForEventComment(INotificationRepository notificationRepository, Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task NotifyForNewPost(INotificationRepository notificationRepository, Post newPost)
        {
            throw new NotImplementedException();
        }
    }
}
