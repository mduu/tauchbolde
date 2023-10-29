using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.DataGateways
{
    public interface IEventRepository: IRepository<Event>
    {
        /// <summary>
        /// Gets the list of upcoming event without the deleted
        /// and canceled events.
        /// </summary>
        /// <returns>All upcoming events</returns>
        Task<List<Event>> GetUpcomingEventsAsync();

        Task<ICollection<Event>> GetUpcomingAndRecentEventsAsync();
    }
}
