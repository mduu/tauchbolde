using System.Collections.Generic;

namespace Tauchbolde.Common.Domain.PhotoStorage
{
    internal class PhotoCategoryConfig
    {
        public static IDictionary<PhotoCategory, PhotoCategoryConfig> Configs =
            new Dictionary<PhotoCategory, PhotoCategoryConfig>
            {
                { PhotoCategory.LogbookTeaser, new PhotoCategoryConfig 
                    {
                        ThumbMaxWidth = 300,
                        ThumbMaxHeight = 300,
                    }
                }
            };

        internal int ThumbMaxWidth { get; private set; }
        internal int ThumbMaxHeight { get; private set; }
    }
}