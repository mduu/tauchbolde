using System.IO;
using System.Threading.Tasks;

namespace Tauchbolde.Application.Services.Avatars
{
    /// <summary>
    /// Provides the main functionallity for working with avatars.
    /// </summary>
    public interface IAvatarStore
    {
        Task<byte[]> GetAvatarBytesAsync(string avatarId);
        Task<string> StoreAvatarAsync(string firstName, string oldAvatarId, string fileExt, Stream fileContent);
    }
}
