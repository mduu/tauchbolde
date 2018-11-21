using System;
using System.IO;
using System.Threading.Tasks;

namespace Tauchbolde.Common.DomainServices.Avatar
{

    public class AvatarStore : IAvatarStore
    {
        private readonly IAvatarIdGenerator avatarIdGenerator;
        private readonly IAvatarPathProvider avatarPathProvider;

        public AvatarStore(
            IAvatarIdGenerator avatarIdGenerator,
            IAvatarPathProvider avatarPathProvider)
        {
            this.avatarIdGenerator = avatarIdGenerator ?? throw new ArgumentNullException(nameof(avatarIdGenerator));
            this.avatarPathProvider = avatarPathProvider ?? throw new ArgumentNullException(nameof(avatarPathProvider));
        }
        
        /// <inheritdoc/>
        public async Task<byte[]> GetAvatarBytesAsync(string avatarId)
        {
            var avatarPath = avatarPathProvider.MapPath(avatarId);
            return await File.ReadAllBytesAsync(avatarPath);      
        }

        /// <inheritdoc/>
        public async Task<string> StoreAvatarAsync(string firstName, string oldAvatarId, string fileExt, Stream fileContent)
        {
            var newAvatarId = avatarIdGenerator.Generate(firstName, fileExt);

            // TODO Resize Avatar
            await StoreNewAvatarAsync(fileContent, newAvatarId);
            if (!string.IsNullOrWhiteSpace(oldAvatarId))
            {
                DeleteAllByAvatarId(oldAvatarId);
            }

            return newAvatarId;
        }

        private string[] GetAvatarFiles(string avatarId)
        {
            return Directory.GetFiles(
                avatarPathProvider.GetAvatarPath(),
                Path.ChangeExtension(avatarId, "*"));
        }

        private async Task StoreNewAvatarAsync(Stream fileContent, string newAvatarId)
        {
            var avatarPath = avatarPathProvider.MapPath(newAvatarId);
            using (var fs = new FileStream(avatarPath, FileMode.Create))
            {
                await fileContent.CopyToAsync(fs);
            }
        }

        private void DeleteAllByAvatarId(string avatarId)
        {
            var existingAvatarFiles = GetAvatarFiles(avatarId);

            foreach (var existingAvatarFile in existingAvatarFiles)
            {
                try
                {
                    File.Delete(existingAvatarFile);
                }
                catch (IOException)
                { }
            }
        }
    }
}
