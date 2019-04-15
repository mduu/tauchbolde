using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic.CompilerServices;

namespace Tauchbolde.Common.DomainServices.PhotoStorage
{
    internal static class ThumbnailConfigurations
    {
        private static readonly IReadOnlyDictionary<ThumbnailType, ThumbnailConfiguration> Configurations =
            new ReadOnlyDictionary<ThumbnailType, ThumbnailConfiguration>(
                new Dictionary<ThumbnailType, ThumbnailConfiguration>
                {
                    {ThumbnailType.LogbookTeaser, new ThumbnailConfiguration(300, 300)},
                }
            );

        internal static ThumbnailConfiguration Get(ThumbnailType thumbnailType)
            => Configurations[thumbnailType];
    }
}