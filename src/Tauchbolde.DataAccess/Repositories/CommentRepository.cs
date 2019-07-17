using Tauchbolde.Entities;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.DataAccess.Repositories
{
    internal class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
