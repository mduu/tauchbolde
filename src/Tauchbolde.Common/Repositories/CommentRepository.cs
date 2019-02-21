using System;
using Tauchbolde.Common.Model;
namespace Tauchbolde.Common.Repositories
{
    internal class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
