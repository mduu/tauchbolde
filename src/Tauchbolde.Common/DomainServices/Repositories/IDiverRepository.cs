using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Repositories
{
    internal interface IDiverRepository: IRepository<Diver>
    {
        /// <summary>
        /// Finds a <see cref="Diver"/> by its username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The user if found; otherwise <c>Null</c> will be returned.</returns>
        Task<Diver> FindByUserNameAsync(string username);

        /// <summary>
        /// Get a collection of all users that are members of Tauchbolde.
        /// </summary>
        /// <returns>A collection of all users that are members of Tauchbolde.</returns>
        Task<ICollection<Diver>> GetAllTauchboldeUsersAsync(bool includingAdmins = false);

        /// <summary>
        /// Get all <see cref="Diver"/> entities including the <see cref="Diver.User"/>
        /// relation loaded.
        /// </summary>
        /// <returns>The all divers.</returns>
        Task<ICollection<Diver>> GetAllDiversAsync();
    }
}
