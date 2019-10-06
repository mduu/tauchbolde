namespace Tauchbolde.Web.Core.TokenHandling
{
    public class TokenConfiguration
    {
        public TokenConfiguration(string tokenSecurityKey)
        {
            TokenSecurityKey = tokenSecurityKey;
        }
        
        public string TokenSecurityKey { get; }
    }
}