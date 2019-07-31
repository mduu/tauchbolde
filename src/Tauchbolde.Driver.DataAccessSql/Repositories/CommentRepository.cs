using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    internal class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
