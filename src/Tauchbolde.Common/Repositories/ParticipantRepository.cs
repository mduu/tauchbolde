using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Repositories
{
    public class ParticipantRepository : RepositoryBase<Participant>, IParticipantRepository
    {
        public ParticipantRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
