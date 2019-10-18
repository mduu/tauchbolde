using Tauchbolde.Application.UseCases.Profile.GetEditAvatarUseCase;

namespace Tauchbolde.InterfaceAdapters.Profile.GetEditAvatar
{
    public class MvcGetEditAvatarPresenter : IGetEditAvatarOutputPort
    {
        private MvcGetEditAvatarViewModel viewModel;
        
        public void Output(GetEditAvatarOutput interactorOutput)
        {
            viewModel = new MvcGetEditAvatarViewModel(interactorOutput.UserId, interactorOutput.Realname);
        }

        public MvcGetEditAvatarViewModel GetViewModel() => viewModel;
    }
}