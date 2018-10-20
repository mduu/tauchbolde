using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.DomainServices;
using Tauchbolde.Common;
using Tauchbolde.Common.Repositories;
using Tauchbolde.Web.Models.UserProfileModels;

namespace Tauchbolde.Web.Controllers
{
    [Route("/profil")]
    [Authorize(Policy = PolicyNames.RequireTauchbold)]
    public class UserProfileController : Controller
    {
        private readonly IDiverRepository diverRepository;
        private readonly IDiverService diverService;

        public UserProfileController(
            IDiverRepository diverRepository,
            IDiverService diverService)
        {
            this.diverRepository = diverRepository ?? throw new ArgumentNullException(nameof(diverRepository));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
        }

        // GET: /profil/<id>
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var member = await diverService.GetMemberAsync(diverRepository, id);
            if (member == null)
            {
                return NotFound();
            }

            var viewModel = new ReadProfileModel
            {
                AllowEdit = false,
                Profile = member,
            };

            return View(viewModel);
        }

        //// GET: /profil/edit
        //[HttpGet]
        //public IActionResult Edit()
        //{
        //    return View();
        //}

        //// GET: /profil/edit
        //[HttpPost]
        //public IActionResult Edit(Diver diver)
        //{
        //    return View();
        //}
    }
}
