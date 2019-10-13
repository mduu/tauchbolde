using System;

namespace Tauchbolde.InterfaceAdapters.UrlBuilders
{
    internal static class FacebookUrlBuilder
    {
        public static string GetUrl(string facebookId) => 
            !string.IsNullOrWhiteSpace(facebookId) 
                ? new Uri($"https://facebook.com/{facebookId}" ).AbsoluteUri 
                : "";
    }
}