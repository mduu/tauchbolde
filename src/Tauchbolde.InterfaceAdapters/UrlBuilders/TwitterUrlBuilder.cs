using System;

namespace Tauchbolde.InterfaceAdapters.UrlBuilders
{
    internal static class TwitterUrlBuilder
    {
        public static string GetUrl(string twitterHandle)
        {
            if (string.IsNullOrWhiteSpace(twitterHandle)) { return ""; }

            var twitterUrl = twitterHandle;

            if (twitterHandle.StartsWith("@", StringComparison.CurrentCulture))
            {
                twitterUrl = twitterHandle.Substring(1);
            }

            return $"https://twitter.com/{twitterUrl}";
        }
    }
}