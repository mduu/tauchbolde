using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Profile.GetEditUserProfileUseCase;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Profile.GetEditUserProfile
{
    public class MvcEditUserProfile : IGetEditUserProfileOutputPort
    {
        private MvcEditUserProfileViewModel viewModel;
        
        public void Output([NotNull] GetEditUserProfileOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));
            
            viewModel = new MvcEditUserProfileViewModel(
                interactorOutput.UserId,
                interactorOutput.Username,
                interactorOutput.Fullname,
                interactorOutput.Firstname,
                interactorOutput.Lastname,
                interactorOutput.Slogan,
                interactorOutput.Education,
                interactorOutput.Experience,
                interactorOutput.MobilePhone,
                interactorOutput.WebsiteUrl,
                interactorOutput.TwitterHandle,
                interactorOutput.FacebookId,
                interactorOutput.SkypeId);
        }

        public MvcEditUserProfileViewModel GetViewModel() => viewModel;
    }
}