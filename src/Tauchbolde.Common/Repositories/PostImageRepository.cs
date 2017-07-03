using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Repositories
{
    public class PostImageRepository : RepositoryBase<PostImage>, IPostImageRepository
    {
        public PostImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
