namespace Tauchbolde.Domain.Types
{
    public class PhotoCategoryConfig
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

        public int ThumbMaxWidth { get; private set; }
        public int ThumbMaxHeight { get; private set; }
    }
}