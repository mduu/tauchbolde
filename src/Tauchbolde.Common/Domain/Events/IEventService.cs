using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Domain.Events
{
    public interface IEventService
    {
        /// <summary>
        /// Returns a stream with the content of a .ical file for the given event.
        /// </summary>
        /// <returns>The ical data in a stream for the event.</returns>
        /// <param name="eventId">Event ID to get the .ical file for.</param>
        /// <param name="createTime">Optional created DateTime. If noll current time will be used.</param>
        Task<Stream> CreateIcalForEventAsync(Guid eventId, DateTime? createTime = null);

        /// <summary>
        /// Insert or update the given <paramref name="eventToUpsert"/>.
        /// </summary>
        /// <param name="eventToUpsert">Eventdata to insert or update.</param>
        /// <param name="currentUser">The current users diver record.</param>
        /// <returns>The inserted or updated <see cref="Event"/>.</returns>
        Task<Event> UpsertEventAsync(Event eventToUpsert, Diver currentUser);

        /// <summary>
        /// Adds the comment to a event async.
        /// </summary>
        /// <returns>The comment that was added.</returns>
        /// <param name="eventId">The Id of the event to add the comment to.</param>
        /// <param name="commentToAdd">Comment to add to the event.</param>
        Task<Comment> AddCommentAsync(Guid eventId, string commentToAdd, Diver authorDiver);

        /// <summary>
        /// Edits the comment async.
        /// </summary>
        /// <returns>The edited comment async.</returns>
        /// <param name="commentId">The Id of the comment to edit.</param>
        /// <param name="commentText">New comment text.</param>
        /// <param name="currentUser">Current user.</param>
        Task<Comment> EditCommentAsync(Guid commentId, string commentText, Diver currentUser);

        /// <summary>
        /// Deletes a comment async.
        /// </summary>
        /// <param name="commentId">Id of the comment to delete.</param>
        /// <param name="currentUser">Current user.</param>
        Task DeleteCommentAsync(Guid commentId, Diver currentUser);

        /// <summary>
        /// Gets upcoming and recent events.
        /// </summary>
        /// <returns>Upcoming and recent events.</returns>
        Task<ICollection<Event>> GetUpcomingAndRecentEventsAsync();

        /// <summary>
        /// Gets upcoming events.
        /// </summary>
        /// <returns>Upcoming events.</returns>
        Task<ICollection<Event>> GetUpcomingEventsAsync();

        /// <summary>
        /// Get the event with all details by its ID.
        /// </summary>
        /// <param name="eventId">The <see cref="Event.Id"/> of the event </param>
        /// <returns>The event with all details by its ID.</returns>
        Task<Event> GetByIdAsync(Guid eventId);
    }
}