namespace Tauchbolde.InterfaceAdapters.UrlBuilders
{
    public static class FacebookUrlBuilder
    {
        public static string GetUrl(string facebookId) => 
            !string.IsNullOrWhiteSpace(facebookId) 
                ? new Uri($"https://facebook.com/{facebookId}" ).AbsoluteUri 
                : "";
    }
}