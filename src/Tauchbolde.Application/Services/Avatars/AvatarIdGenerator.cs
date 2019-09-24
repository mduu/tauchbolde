using System;
using System.IO;
using System.Linq;

namespace Tauchbolde.Application.Services.Avatars
{
    /// <summary>
    /// Standard implementation of <see cref="IAvatarIdGenerator"/>.
    /// </summary>
    /// <seealso cref="IAvatarIdGenerator"/>
    public class AvatarIdGenerator : IAvatarIdGenerator
    {
        private readonly IAvatarPathProvider avatarPathProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tauchbolde.Application.Services.Avatars.AvatarIdGenerator"/> class.
        /// </summary>
        /// <param name="avatarPathProvider">Avatar path provider.</param>
        public AvatarIdGenerator(IAvatarPathProvider avatarPathProvider)
        {
            this.avatarPathProvider = avatarPathProvider ?? throw new ArgumentNullException(nameof(avatarPathProvider));
        }

        /// <inheritdoc/>
        public string Generate(string firstName, string fileExt)
        {
            var isUniqueId = false;
            var counter = 1;
            do
            {
                var avatarId = $"{firstName}_{counter}";
                var avatarPath = Path.ChangeExtension(
                    avatarPathProvider.MapPath(avatarId),
                    fileExt);

                isUniqueId = !(Directory.GetFiles(
                    Path.GetDirectoryName(avatarPath),
                    Path.ChangeExtension(avatarId, "*")).Any());
                if (isUniqueId) {
                    return Path.ChangeExtension(avatarId, fileExt);
                }
                
                counter++;
            } while (true);
        }
    }
}
