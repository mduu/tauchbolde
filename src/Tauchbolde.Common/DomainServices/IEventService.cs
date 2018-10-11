﻿using System;
using System.IO;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Common.DomainServices
{
    public interface IEventService
    {
        /// <summary>
        /// Returns a stream with the content of a .ical file for the given event.
        /// </summary>
        /// <returns>The ical data in a stream for the event.</returns>
        /// <param name="eventId">Event ID to get the .ical file for.</param>
        /// <param name="eventRepository">Event repository.</param>
        Task<Stream> CreateIcalForEvent(Guid eventId, IEventRepository eventRepository);

        /// <summary>
        /// Insert or update the given <paramref name="eventToUpsert"/>.
        /// </summary>
        /// <param name="eventRepository"></param>
        /// <param name="eventToUpsert">Eventdata to insert or update.</param>
        /// <returns>The inserted or updated <see cref="Event"/>.</returns>
        Task<Event> UpsertEventAsync(IEventRepository eventRepository, Event eventToUpsert);

        /// <summary>
        /// Adds the comment to a event async.
        /// </summary>
        /// <returns>The comment that was added.</returns>
        /// <param name="eventId">The Id of the event to add the comment to.</param>
        /// <param name="commentToAdd">Comment to add to the event.</param>
        Task<Comment> AddCommentAsync(Guid eventId, string commentToAdd, Diver authorDiver, ICommentRepository commentRepositor);
    }
}