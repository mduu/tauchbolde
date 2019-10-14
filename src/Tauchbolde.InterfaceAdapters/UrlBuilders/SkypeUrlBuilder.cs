using System;

namespace Tauchbolde.InterfaceAdapters.UrlBuilders
{
    internal static class SkypeUrlBuilder
    {
        public static string GetUrl(string skypeId) =>
            !string.IsNullOrWhiteSpace(skypeId) 
                ? new Uri($"skype:{skypeId}" ).AbsoluteUri
                : "";
    }
}