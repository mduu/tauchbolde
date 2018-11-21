﻿using System.IO;
using System.Threading.Tasks;

namespace Tauchbolde.Common.DomainServices.Avatar
{
    /// <summary>
    /// Provides the main functionallity for working with avatars.
    /// </summary>
    public interface IAvatarStore
    {
        Task<byte[]> GetAvatarBytes(string avatarId);
        Task<string> StoreAvatarAsync(string firstName, string oldAvatarId, string fileExt, Stream fileContent);
    }
}
