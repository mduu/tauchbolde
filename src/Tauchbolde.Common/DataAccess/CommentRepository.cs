using Tauchbolde.Common.Domain.Repositories;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DataAccess
{
    internal class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
