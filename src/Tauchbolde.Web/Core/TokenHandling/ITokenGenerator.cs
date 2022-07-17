namespace Tauchbolde.Web.Core.TokenHandling
{
    public interface ITokenGenerator
    {
        Task<object> CreateTokenAsync(string username, string password);
    }
}