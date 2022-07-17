using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tauchbolde.Web.Core;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.Application.Services;
using Tauchbolde.Application.Services.Avatars;
using Tauchbolde.Application.UseCases.Profile.EditAvatarUseCase;
using Tauchbolde.Application.UseCases.Profile.EditUserProfileUseCase;
using Tauchbolde.Application.UseCases.Profile.GetEditAvatarUseCase;
using Tauchbolde.Application.UseCases.Profile.GetEditUserProfileUseCase;
using Tauchbolde.Application.UseCases.Profile.GetUserProfileUseCase;
using Tauchbolde.InterfaceAdapters.MVC.Presenters.Profile.GetEditAvatar;
using Tauchbolde.InterfaceAdapters.MVC.Presenters.Profile.GetEditUserProfile;
using Tauchbolde.InterfaceAdapters.MVC.Presenters.Profile.GetUserProfile;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Web.Controllers
{
    // TODO Convert these actions into proper use-case interactors and presenters
    [Route("/profil")]
    [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
    public class UserProfileController : AppControllerBase
    {
        [NotNull] private readonly IMediator mediator;
        [NotNull] private readonly IMimeMapping mimeMapping;
        [NotNull] private readonly IAvatarStore avatarStore;

        public UserProfileController(
            [NotNull] IMediator mediator,
            [NotNull] IMimeMapping mimeMapping,
            [NotNull] IAvatarStore avatarStore)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.mimeMapping = mimeMapping ?? throw new ArgumentNullException(nameof(mimeMapping));
            this.avatarStore = avatarStore ?? throw new ArgumentNullException(nameof(avatarStore));
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
            return RedirectToAction("Index", new {id = model.UserId});
        }

        [Route("editavatar/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditAvatar(Guid id)
        {
            var presenter = new MvcGetEditAvatarPresenter();
            var useCaseResult = await mediator.Send(new GetEditAvatar(id, presenter));
            if (useCaseResult.IsSuccessful)
            {
                return View(presenter.GetViewModel());
            }

            var map = new Dictionary<ResultCategory, Func<IActionResult>>
            {
                {ResultCategory.NotFound, NotFound},
                {ResultCategory.AccessDenied, Forbid},
                {ResultCategory.GeneralFailure, () => StatusCode(500)},
            };

            return map[useCaseResult.ResultCategory]();
        }

        [Route("editavatar/{id}")]
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(Guid id, List<IFormFile> imgInp)
        {
            try
            {
                var file = Request.Form.Files.First(f => f.Length > 0);
                var avatar = new EditAvatar.AvatarFile(file.FileName, file.ContentType, file.OpenReadStream());
                var useCaseResult = await mediator.Send(new EditAvatar(id, avatar));
                var resultMessageMap = new Dictionary<ResultCategory, Action>
                {
                    {ResultCategory.AccessDenied, () => ShowErrorMessage("Du hast keine Berechtigungen!")},
                    {ResultCategory.NotFound, () => ShowErrorMessage("Taucher nicht gefunden!")},
                    {ResultCategory.GeneralFailure, () => ShowErrorMessage("Ein Fehler ist aufgetreten!")},
                    {ResultCategory.Success, () => ShowSuccessMessage("Profilbild erfolgreich aktualisiert.")},
                };
                
                resultMessageMap[useCaseResult.ResultCategory]();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Fehler beim Aktualisieren des Profilbildes: {ex.Message}");
            }

            return RedirectToAction("Index", new {id});
        }
    }
}