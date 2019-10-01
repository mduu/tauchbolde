namespace Tauchbolde.Application.Services.Avatars
{
    /// <summary>
    /// Generates new Avatar-ID's.
    /// </summary>
    public interface IAvatarIdGenerator
    {
        /// <summary>
        /// Generate a new avatarId for the given <paramref name="firstName"/> and <paramref name="fileExt"/>.
        /// </summary>
        /// <returns>The generate.</returns>
        /// <param name="firstName">Diver data to generate the avatar ID for.</param>
        /// <param name="fileExt">File extension to use for the ID.</param>
        string Generate(string firstName, string fileExt);
    }
}
