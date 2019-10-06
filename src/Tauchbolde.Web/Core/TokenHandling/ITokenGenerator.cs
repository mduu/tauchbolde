using System.Threading.Tasks;

namespace Tauchbolde.Web.Core.TokenHandling
{
    public interface ITokenGenerator
    {
        Task<object> CreateTokenAsync(string username, string password);
    }
}