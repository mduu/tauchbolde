using System;
using System.Linq;
using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Profile.MemberListUseCase;
using Tauchbolde.InterfaceAdapters.UrlBuilders;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.InterfaceAdapters.Profile.MemberList
{
    public class MvcMemberListPresenter : IMemberListOutputPort
    {
        private MvcMemberListViewModel viewModel;
        
        public void Output([NotNull] MemberListOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));
            
            viewModel = new MvcMemberListViewModel(
                interactorOutput.AllowSeeDetails,
                interactorOutput.Members.Select(m => new MvcMemberListViewModel.MvcMemberViewModel(
                    m.DiverId,
                    m.Email,
                    m.AvatarId,
                    m.Name,
                    m.MemberSince.ToStringSwissDate(),
                    m.Education,
                    m.Experience,
                    m.Slogan,
                    m.WebsiteUrl,
                    m.TwitterHandle,
                    TwitterUrlBuilder.GetUrl(m.TwitterHandle),
                    m.FacebookId,
                    FacebookUrlBuilder.GetUrl(m.FacebookId))));
        }

        public MvcMemberListViewModel GetViewModel() => viewModel;
    }
}