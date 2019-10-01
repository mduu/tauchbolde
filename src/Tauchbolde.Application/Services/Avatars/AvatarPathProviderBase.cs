using System.IO;

namespace Tauchbolde.Application.Services.Avatars
{
    /// <summary>
    /// Implements base class for <see cref="IAvatarPathProvider"/>.
    /// </summary>
    /// <seealso cref="IAvatarPathProvider"/>
    /// <seealso cref="IAvatarStore"/>
    public abstract class AvatarPathProviderBase : IAvatarPathProvider
    {
        /// <inheritdoc/>
        public string MapPath(string avatarId)
        {
            EnsurePathExists();
            var filePath = Path.Combine(GetAvatarPath(), avatarId);
                
            return filePath;
        }

        /// <inheritdoc/>
        public string GetAvatarPath()
        {
            EnsurePathExists();
            return InternGetAvatarPath();
        }

        /// <summary>
        /// Ensures the directoy for the path exists.
        /// </summary>
        protected void EnsurePathExists()
        {
            Directory.CreateDirectory(InternGetAvatarPath());
        }

        protected abstract string GetRootPath();
        
        private string InternGetAvatarPath() => Path.Combine(GetRootPath(), "avatar");
    }

}
