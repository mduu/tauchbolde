namespace Tauchbolde.InterfaceAdapters.UrlBuilders
{
    public static class EmailUrlBuilder
    {
        public static string GetUrl(string emailAddress) =>
            string.IsNullOrWhiteSpace(emailAddress)
                ? ""
                : $"mailto:{emailAddress}";
    }
}