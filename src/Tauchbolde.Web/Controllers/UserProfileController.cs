using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tauchbolde.Web.Models.UserProfileModels;
using Tauchbolde.Web.Core;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.OldDomainServices.Users;
using Tauchbolde.Application.Services;
using Tauchbolde.Application.Services.Avatars;
using Tauchbolde.Application.Services.Core;
using Tauchbolde.Application.UseCases.Profile.EditUserProfileUseCase;
using Tauchbolde.Application.UseCases.Profile.GetEditUserProfileUseCase;
using Tauchbolde.Application.UseCases.Profile.GetUserProfileUseCase;
using Tauchbolde.Driver.DataAccessSql;
using Tauchbolde.Domain.Entities;
using Tauchbolde.InterfaceAdapters.Profile.GetEditUserProfile;
using Tauchbolde.InterfaceAdapters.Profile.GetUserProfile;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Web.Controllers
{
    // TODO Convert these actions into proper use-case interactors and presenters
    [Route("/profil")]
    [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
    public class UserProfileController : AppControllerBase
    {
        [NotNull] private readonly IMediator mediator;
        [NotNull] private readonly ApplicationDbContext context;
        [NotNull] private readonly IDiverService diverService;
        [NotNull] private readonly IMimeMapping mimeMapping;
        [NotNull] private readonly IAvatarStore avatarStore;
        [NotNull] private readonly ICurrentUser currentUser;

        public UserProfileController(
            [NotNull] IMediator mediator,
            [NotNull] ApplicationDbContext context,
            [NotNull] IDiverService diverService,
            [NotNull] IMimeMapping mimeMapping,
            [NotNull] IAvatarStore avatarStore,
            [NotNull] ICurrentUser currentUser)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.diverService = diverService ?? throw new ArgumentNullException(nameof(diverService));
            this.mimeMapping = mimeMapping ?? throw new ArgumentNullException(nameof(mimeMapping));
            this.avatarStore = avatarStore ?? throw new ArgumentNullException(nameof(avatarStore));
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        // GET: /profil/<id>
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var presenter = new MvcGetUserProfilePresenter();
            var useCaseResult = await mediator.Send(new GetUserProfile(id, presenter));
            if (!useCaseResult.IsSuccessful)
            {
                return useCaseResult.ResultCategory == ResultCategory.NotFound
                    ? NotFound()
                    : StatusCode(500);
            }
            
            return View(presenter.GetViewModel());
        }

        // GET: /profil/edit
        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var presenter = new MvcEditUserProfile();
            var useCaseResult = await mediator.Send(new GetEditUserProfile(id, presenter));
            if (!useCaseResult.IsSuccessful)
            {
                return useCaseResult.ResultCategory == ResultCategory.NotFound 
                    ? NotFound()
                    : StatusCode(500);
            }

            return base.View(presenter.GetViewModel());
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
        public async Task<IActionResult> Edit(Guid id, MvcEditUserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var useCaseResult = await mediator.Send(new EditUserProfile(
                model.UserId,
                model.Fullname,
                model.Firstname,
                model.Lastname,
                model.Slogan,
                model.Education,
                model.Experience,
                model.MobilePhone,
                model.WebsiteUrl,
                model.TwitterHandle,
                model.FacebookId,
                model.SkypeId));

            if (!useCaseResult.IsSuccessful)
            {
                var errorMsg = new Dictionary<ResultCategory, string>
                {
                    {ResultCategory.AccessDenied, "Sie haben keine Berechtigung diese Benutzerdaten zu ändern!"},
                    {ResultCategory.NotFound, "Zu bearbeitende Benutzerdaten nicht gefunden!"},
                    {ResultCategory.GeneralFailure, "Ein Fehler beim Speichern der Benutzerdaten ist aufgetreten!"},
                };

                ShowErrorMessage(errorMsg[useCaseResult.ResultCategory]);
                return View(model);
            }
                
            ShowSuccessMessage("Profil wurde gespeichert.");
            return RedirectToAction("Index", new { id = model.UserId });

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
            public Diver Member { get; }
            public bool HasWriteAccess =>  Member.Id == CurrentDiver.Id || CurrentDiverIsAdmin;
        }

        private async Task<MemberContext> GetMemberContextAsync(Guid diverId)
        {
            var currentDiver = await currentUser.GetCurrentDiverAsync();
            var isAdmin = await currentUser.GetIsAdminAsync();
            var member = await diverService.GetMemberAsync(diverId);

            return new MemberContext(currentDiver, isAdmin, member);
        }
    }
}
