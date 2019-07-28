using Tauchbolde.Application.DataGateways;
using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.DataEntities;
using Tauchbolde.Driver.DataAccessSql.Mappers;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    internal class CommentRepository : RepositoryBase<Comment, CommentData>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override Comment MapTo(CommentData dataEntity) => dataEntity.MapTo();
        protected override CommentData MapTo(Comment domainEntity) => domainEntity.MapTo();
    }
}
