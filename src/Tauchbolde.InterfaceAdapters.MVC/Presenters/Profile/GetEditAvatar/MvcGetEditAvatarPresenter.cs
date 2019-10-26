using Tauchbolde.Application.Services.Avatars;
using Tauchbolde.Application.UseCases.Profile.GetEditAvatarUseCase;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Profile.GetEditAvatar
{
    public class MvcGetEditAvatarPresenter : IGetEditAvatarOutputPort
    {
        private MvcGetEditAvatarViewModel viewModel;

        public void Output(GetEditAvatarOutput interactorOutput)
        {
            viewModel = new MvcGetEditAvatarViewModel(
                interactorOutput.UserId,
                interactorOutput.Realname,
                AvatarConstants.SizeSm,
                AvatarConstants.SizeMd);
        }

        public MvcGetEditAvatarViewModel GetViewModel() => viewModel;
    }
}