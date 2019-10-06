using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Web.Api.Controllers
{
    [Route("api/token")]
    public class TokenController : Controller
    {
        [NotNull] private readonly UserManager<IdentityUser> userManager;
        [NotNull] private readonly SignInManager<IdentityUser> signInManager;
        private readonly ILogger logger;

        public TokenController(
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] SignInManager<IdentityUser> signInManager,
            [NotNull] ILogger<TokenController> logger)
        {
            if (signInManager == null) throw new ArgumentNullException(nameof(signInManager));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("login")]
        [HttpPost]        
        public async Task<IActionResult> Create(string username, string password)
        {
            if (!await IsValidUserAndPasswordCombination(username, password))
            {
                return BadRequest();
            }
            
            return new ObjectResult(await GenerateTokenAsync(username));

        }

        private async Task<bool> IsValidUserAndPasswordCombination(string username, string password)
        {
            var user = await GetIdentityUserAsync(username);
            if (user == null)
            {
                logger.LogWarning("API-Login: User with username {username} not found by UserManager!", username);
                return false;
            }
            
            if (!user.EmailConfirmed)
            {
                logger.LogWarning("API-Login: User with username {username} has not yet confirmed his email-address!", username);
                return false;
            }

            var passwordSignInResult = await signInManager.PasswordSignInAsync(user, password, false, false);
            if (!passwordSignInResult.Succeeded)
            {
                logger.LogWarning("API-Login: Password invalid or user locked out! Username: {username}", username);
                return false;
            }

            return true;
        }
        
        private async Task<object> GenerateTokenAsync(string username)
        {
            var user = await GetIdentityUserAsync(username);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };
            
            if (await userManager.IsInRoleAsync(user, Rolenames.Tauchbold))
            {
                claims.Add(new Claim(ClaimTypes.Role, Rolenames.Tauchbold));
            }

            if (await userManager.IsInRoleAsync(user, Rolenames.Administrator))
            {
                claims.Add(new Claim(ClaimTypes.Role, Rolenames.Administrator));
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TODO a secret that needs to be at least 16 characters long"));

            var token = new JwtSecurityToken(
                issuer: nameof(Tauchbolde),
                audience: "everybody",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        private async Task<IdentityUser> GetIdentityUserAsync(string username) => await userManager.FindByNameAsync(username);
    }
}