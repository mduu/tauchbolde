using System;
using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Profile.GetUserProfileUseCase;
using Tauchbolde.InterfaceAdapters.UrlBuilders;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Profile.GetUserProfile
{
    [UsedImplicitly]
    public class MvcGetUserProfilePresenter : IGetUserProfileOutputPort
    {
        private MvcUserProfileViewModel viewModel;
        
        public void Output([NotNull] GetUserProfileOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));
            
            viewModel = new MvcUserProfileViewModel(
                interactorOutput.AllowEdit,
                interactorOutput.UserId,
                interactorOutput.Roles,
                interactorOutput.Email,
                EmailUrlBuilder.GetUrl(interactorOutput.Email),
                interactorOutput.AvatarId,
                interactorOutput.Realname,
                interactorOutput.Firstname,
                interactorOutput.Lastname,
                interactorOutput.MemberSince.ToStringSwissDate(),
                interactorOutput.Slogan,
                interactorOutput.Education,
                interactorOutput.Experience,
                interactorOutput.MobilePhone,
                PhoneUrlBuilder.GetUrl(interactorOutput.MobilePhone),
                interactorOutput.WebsiteUrl,
                interactorOutput.TwitterHandle,
                TwitterUrlBuilder.GetUrl(interactorOutput.TwitterHandle),
                interactorOutput.FacebookId,
                FacebookUrlBuilder.GetUrl(interactorOutput.FacebookId),
                interactorOutput.SkypeId,
                SkypeUrlBuilder.GetUrl(interactorOutput.SkypeId));
        }

        public MvcUserProfileViewModel GetViewModel() => viewModel;
    }
}