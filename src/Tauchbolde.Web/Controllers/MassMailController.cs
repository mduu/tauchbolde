using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common;
using Tauchbolde.Web.Core;
using System.Threading.Tasks;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Common.Model;
using System.Collections.Generic;
using System.Linq;
using Tauchbolde.Web.Models.MassMail;

namespace Tauchbolde.Web.Controllers
{

    [Authorize(Policy = PolicyNames.RequireTauchbold)]
    [Authorize(Policy = PolicyNames.RequireAdministrator)]
    public class MassMailController : AppControllerBase
    {
        private readonly IDiverRepository diverRepository;

        public MassMailController(IDiverRepository diverRepository)
        {
            this.diverRepository = diverRepository ?? throw new System.ArgumentNullException(nameof(diverRepository));
        }

        // GET: /MassMail/
        public async Task<IActionResult> Index()
        {
            var tauchbolde = await diverRepository.GetAllTauchboldeUsersAsync();

            return base.View(new MassMailViewModel
            {
                TauchboldeEmailReceiver = CreateReceiver(tauchbolde)
            });
        }

        // TODO Write unit-tests for this function
        private static string CreateReceiver(ICollection<Diver> divers)
        {
            return string.Join(
                ";",
                divers.Select(d => $"\"{d.Fullname}\"<{d.User.Email}>"));
        }
    }
}
