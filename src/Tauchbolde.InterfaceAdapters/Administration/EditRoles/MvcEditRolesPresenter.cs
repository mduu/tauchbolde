using System;
using System.Linq;
using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Administration.GetEditRolesUseCase;

namespace Tauchbolde.InterfaceAdapters.Administration.EditRoles
{
    public class MvcEditRolesPresenter : IGetEditRolesOutputPort
    {
        private MvcEditRolesViewModel viewModel;
        
        public void Output([NotNull] GetEditRolesOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));
            
            viewModel = new MvcEditRolesViewModel(
                interactorOutput.UserName,
                interactorOutput.FullName,
                interactorOutput.AllRoles.Select(r => r),
                interactorOutput.AssignedRoles.Select(r => r));
        }

        public MvcEditRolesViewModel GetViewModel() => viewModel;
    }
}