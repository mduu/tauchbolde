using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tauchbolde.Web.Core.TokenHandling;

namespace Tauchbolde.Web.Api.Controllers
{
    [Route("api/v1/token")]
    public class TokenController : Controller
    {
        private readonly ILogger logger;
        private readonly ITokenGenerator tokenGenerator;

        public TokenController(
            [NotNull] ILogger<TokenController> logger,
            [NotNull] ITokenGenerator tokenGenerator)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
        }

        [Route("login")]
        [HttpPost]        
        public async Task<IActionResult> Create(string username, string password)
        {
            logger.LogInformation("Request for creating new login token for user {usernane}", username);
            
            var token = await tokenGenerator.CreateTokenAsync(username, password);
            
            return token != null
                ? new ObjectResult(token)
                : (IActionResult) BadRequest();
        }
    }
}