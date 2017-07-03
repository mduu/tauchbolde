using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repository;

namespace Tauchbolde.Common.Repositories
{
    /// <summary>
    ///     Defines additional data-access logic for <see cref="Post" />.
    /// </summary>
    public interface IPostRepository : IRepository<Post>
    {
        /// <summary>
        ///     Gets the latest posts for the given <paramref name="category" />.
        ///     With the parameter <paramref name="countPosts" /> the number of
        ///     posts returned can be limited.
        /// </summary>
        /// <param name="category">The category to get the posts for.</param>
        /// <param name="countPosts">
        ///     If <c>0</c> the number posts returned is not limited;
        ///     otherwise only the specified number of posts will be returned.
        /// </param>
        /// <returns></returns>
        Task<ICollection<Post>> GetNewestPostsForCategoryAsync(PostCategory category, int countPosts);

        /// <summary>
        /// Get a <see cref="Post"/> with its sub-entities.
        /// </summary>
        /// <param name="postId">The <see cref="Post.Id"/>.</param>
        /// <returns>The <see cref="Post"/> with its sub-entities.</returns>
        Task<Post> GetPostIncludingImagesAndAuthorAsync(Guid postId);

        /// <summary>
        /// Get all posts including their subentities.
        /// </summary>
        IAsyncEnumerable<Post> GetAllIncludingImagesAndAuthor();
    }
}