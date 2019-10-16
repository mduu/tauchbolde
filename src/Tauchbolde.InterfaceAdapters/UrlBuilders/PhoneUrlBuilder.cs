namespace Tauchbolde.InterfaceAdapters.UrlBuilders
{
    public static class PhoneUrlBuilder
    {
        public static string GetUrl(string phoneNumber) =>
            string.IsNullOrWhiteSpace(phoneNumber)
                ? ""
                : $"tel:{phoneNumber}";
    }
}