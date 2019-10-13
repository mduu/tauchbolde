namespace Tauchbolde.InterfaceAdapters.UrlBuilders
{
    public static class PhoneUrlBuilder
    {
        public static string GetUrl(string phoneNumber) => $"tel:{phoneNumber}";
    }
}