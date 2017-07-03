using System;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;

        public PostService(
            ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
