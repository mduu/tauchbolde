namespace Tauchbolde.Common.DomainServices.Avatar
{
    /// <summary>
    /// Provides path information for storing and accessing avatars.
    /// </summary>
    public interface IAvatarPathProvider
    {
        /// <summary>
        /// Maps a <paramref name="avatarId"/> to a file-system path.
        /// </summary>
        /// <returns>The file-system path for the given <paramref name="avatarId"/>.</returns>
        /// <param name="avatarId">The avatarId to get the path for.</param>
        string MapPath(string avatarId);

        /// <summary>
        /// Get the path where the avatars are stored.
        /// </summary>
        /// <returns>The avatar path.</returns>
        string GetAvatarPath();
    }
}
