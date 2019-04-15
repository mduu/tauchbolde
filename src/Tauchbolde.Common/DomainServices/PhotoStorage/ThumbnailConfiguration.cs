namespace Tauchbolde.Common.DomainServices.PhotoStorage
{
    internal class ThumbnailConfiguration
    {
        public ThumbnailConfiguration(int maxWidth, int maxHeight)
        {
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
        }

        internal int MaxWidth { get; }
        internal int MaxHeight { get; }
    }
}