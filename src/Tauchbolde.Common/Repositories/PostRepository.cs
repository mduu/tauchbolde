using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.Repositories
{
    /// <summary>
    /// Data-access repository for Posts and PostImages.
    /// </summary>
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="PostRepository"/>.
        /// </summary>
        /// <param name="context"></param>
        public PostRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<ICollection<Post>> GetNewestPostsForCategoryAsync(PostCategory category, int countPosts)
        {
            var query = Context.Posts
                .Where(p => p.Category == category)
                .Include(p => p.Author)
                .ThenInclude(a => a.AdditionalUserInfos)
                .OrderByDescending(p => p.PublishDate);

            if (countPosts > 0)
            {
                query.Take(countPosts);
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Post> GetPostIncludingImagesAndAuthorAsync(Guid postId)
        {
            if (postId == Guid.Empty) throw new ArgumentOutOfRangeException(nameof(postId), "Post-ID can not be empty.");

            return await Context.Posts
                .Include(p => p.PostImages)
                .Include(p => p.Author)
                .ThenInclude(a => a.AdditionalUserInfos)
                .FirstOrDefaultAsync(p => p.Id == postId);
        }

        /// <inheritdoc />
        public IAsyncEnumerable<Post> GetAllIncludingImagesAndAuthor()
        {
            return Context.Posts
                .OrderByDescending(p => p.PublishDate)
                .Include(p => p.PostImages)
                .Include(p => p.Author)
                .ThenInclude(a => a.AdditionalUserInfos)
                .ToAsyncEnumerable();
        }
    }
}