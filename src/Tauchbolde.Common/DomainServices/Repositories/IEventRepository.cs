using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Repositories
{
    public interface IEventRepository: IRepository<Event>
    {
        /// <summary>
        /// Gets the list of upcomming event without the deleted
        /// and canceled events.
        /// </summary>
        /// <returns>All upcomming events</returns>
        Task<List<Event>> GetUpcommingEventsAsync();

        Task<ICollection<Event>> GetUpcomingAndRecentEventsAsync();
    }
}
