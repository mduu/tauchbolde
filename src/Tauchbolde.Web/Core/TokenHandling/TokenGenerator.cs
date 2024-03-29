using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Tauchbolde.Domain.Types;

namespace Tauchbolde.Web.Core.TokenHandling
{
    public class TokenGenerator : ITokenGenerator
    {
        [NotNull] private readonly UserManager<IdentityUser> userManager;
        [NotNull] private readonly SignInManager<IdentityUser> signInManager;
        [NotNull] private readonly ILogger logger;
        [NotNull] private readonly TokenConfiguration configuration;

        public TokenGenerator(
            [NotNull] UserManager<IdentityUser> userManager,
            [NotNull] SignInManager<IdentityUser> signInManager,
            [NotNull] ILogger<TokenGenerator> logger,
            [NotNull] TokenConfiguration configuration)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<object> CreateTokenAsync(string username, string password)
        {
            if (!await IsValidUserAndPasswordCombination(username, password))
            {
                return null;
            }
            
            return await GenerateTokenAsync(username);
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

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.TokenSecurityKey));

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