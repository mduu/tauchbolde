using System.IO;

namespace Tauchbolde.Common.DomainServices.Avatar
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
            var filePath = Path.Combine(GetAvatarPath(), avatarId);
                
            return filePath;
        }

        /// <inheritdoc/>
        public string GetAvatarPath() => Path.Combine(GetRootPath(), "avatar");

        protected abstract string GetRootPath();
    }

}
