using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tauchbolde.Common.Model;
using Tauchbolde.Common;
using Tauchbolde.Web.Models.UserProfileModels;
using Microsoft.AspNetCore.Identity;
using Tauchbolde.Web.Core;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Tauchbolde.Commom.Misc;
using System.IO;
using Tauchbolde.Common.Domain.Avatar;
using Tauchbolde.Common.Domain.Users;

namespace Tauchbolde.Web.Controllers
{
    [Route("/profil")]
    [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
    public class UserProfileController : AppControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IDiverService diverService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMimeMapping mimeMapping;
        private readonly IAvatarStore avatarStore;

        public UserProfileController(
            ApplicationDbContext context,
            IDiverService diverService,
            UserManager<IdentityUser> userManager,
            IMimeMapping mimeMapping,
            IAvatarStore avatarStore)
            : base(userManager, diverService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.mimeMapping = mimeMapping ?? throw new ArgumentNullException(nameof(mimeMapping));
            this.avatarStore = avatarStore ?? throw new ArgumentNullException(nameof(avatarStore));
        }

        // GET: /profil/<id>
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var memberContext = await GetMemberContextAsync(id);
            if (memberContext.CurrentDiver == null && memberContext.Member == null)
            {
                return StatusCode(400);
            }

            return View(new ReadProfileModel
            {
                AllowEdit = memberContext.CurrentDiver != null && (memberContext.Member.Id == memberContext.CurrentDiver.Id || memberContext.CurrentDiverIsAdmin),
                Profile = memberContext.Member,
                Roles = await userManager.GetRolesAsync(memberContext.Member.User),
            });
        }

        // GET: /profil/edit
        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var memberContext = await GetMemberContextAsync(id);
            if (memberContext.CurrentDiver == null && memberContext.Member == null)
            {
                return StatusCode(400);
            }

            if (!memberContext.HasWriteAccess)
            {
                return Forbid();
            }

            return base.View(new WriteProfileModel
            {
                Profile = memberContext.Member,
            });
        }
        
        // GET: /avatar/marc_3.jpg
        [Route("avatar/{avatarId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvatar(string avatarId)
        {
            if (string.IsNullOrWhiteSpace(avatarId))
            {
                return BadRequest();
            }

            return File(
                await avatarStore.GetAvatarBytesAsync(avatarId),
                mimeMapping.GetMimeMapping(Path.GetExtension(avatarId)));
        }

        // GET: /profil/edit
        [Route("edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, WriteProfileModel model)
        {
            if (ModelState.IsValid)
            {
                await diverService.UpdateUserProfileAsync(model.Profile);
                await context.SaveChangesAsync();

                ShowSuccessMessage("Profil wurde gespeichert.");
                return RedirectToAction("Index", new { id = model.Profile.Id });
            }

            ShowErrorMessage("Fehler beim Speichern aufgetreten!");

            return View(new WriteProfileModel
            {
                Profile = model.Profile,
            });
        }

        [Route("editavatar/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditAvatar(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var memberContext = await GetMemberContextAsync(id);
            if (memberContext.CurrentDiver == null && memberContext.Member == null)
            {
                return StatusCode(400);
            }

            if (!memberContext.HasWriteAccess)
            {
                return Forbid();
            }

            return base.View(new WriteProfileModel
            {
                Profile = memberContext.Member,
            });
        }

        [Route("editavatar/{id}")]
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(Guid id, List<IFormFile> imgInp)
        {
            try
            {
                if (id == Guid.Empty) { throw new ArgumentException("Guid.Empty now allowed!", nameof(id)); }

                if (Request.Form.Files.Any())
                {
                    var memberContext = await GetMemberContextAsync(id);
                    if (memberContext.CurrentDiver == null && memberContext.Member == null)
                    {
                        return StatusCode(400);
                    }

                    if (!memberContext.HasWriteAccess)
                    {
                        return Forbid();
                    }

                    var file = Request.Form.Files.First(f => f.Length > 0);
                    var fileExt = mimeMapping.GetFileExtensionMapping(file.ContentType);
                    var fileExt1 = fileExt ?? Path.GetExtension(file.FileName);
                    
                    var newAvatarId = await avatarStore.StoreAvatarAsync(
                        memberContext.Member.Firstname,
                        memberContext.Member.AvatarId,
                        fileExt1,
                        file.OpenReadStream());

                    memberContext.Member.AvatarId = newAvatarId;
                    await context.SaveChangesAsync();

                    ShowSuccessMessage("Profilbild erfolgreich aktualisiert.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Fehler beim Aktualisieren des Profilbildes: {ex.Message}");
            }
            
            return RedirectToAction("Index", new { id });
        }

        private class MemberContext
        {
            public MemberContext(Diver currentDiver, bool currentDiverIsAdmin, Diver member)
            {
                CurrentDiver = currentDiver;
                CurrentDiverIsAdmin = currentDiverIsAdmin;
                Member = member;
            }

            public Diver CurrentDiver { get; }
            public bool CurrentDiverIsAdmin { get; }
            public Diver Member { get; set; }
            public bool HasWriteAccess =>
                Member.Id == CurrentDiver.Id || CurrentDiverIsAdmin;


        }

        private async Task<MemberContext> GetMemberContextAsync(Guid diverId)
        {
            var currentDiver = await GetDiverForCurrentUserAsync();
            var isAdmin = await GetIsAdmin();
            var member = await diverService.GetMemberAsync(diverId);

            return new MemberContext(currentDiver, isAdmin, member);
        }
    }
}
