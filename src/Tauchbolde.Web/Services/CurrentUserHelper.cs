using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.Web.Services
{
    public class CurrentUserHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CurrentUserHelper(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public ApplicationUser GetCurrentUser(HttpRequest request)
        {
            var username = _userManager.Find
        }
    }
}
