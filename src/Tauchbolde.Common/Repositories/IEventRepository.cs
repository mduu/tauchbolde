using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repository;

namespace Tauchbolde.Common.Repositories
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
