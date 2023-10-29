using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Administration.GetMemberManagementUseCase;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Administration.MemberManagement
{
    public class MvcMemberManagementPresenter : IMemberManagementOutputPort
    {
        private MvcMemberManagementViewModel viewModel;

        public void Output([NotNull] MemberManagementOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));

            viewModel = new MvcMemberManagementViewModel(
                interactorOutput.Members.Select(m =>
                    new MvcMemberManagementViewModel.MemberViewModel(
                        m.DiverId,
                        m.Fullname,
                        m.UserName,
                        m.Email,
                        m.EmailConfirmed,
                        m.LockoutEnabled,
                        m.Roles.ToList())),
                interactorOutput.AddableUsers.ToList());
        }

        public MvcMemberManagementViewModel GetViewModel() => viewModel;
    }
}