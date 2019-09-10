using System;
using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Event.GetEventEditDetailsUseCase;
using Tauchbolde.SharedKernel.Extensions;

namespace Tauchbolde.InterfaceAdapters.Event.EditDetails
{
    public class MvcEventEditDetailsPresenter : IEventEditDetailsOutputPort
    {
        private MvcEventEditDetailsViewModel viewModel;
        
        public void Output([NotNull] EventEditDetailsOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));
            
            viewModel = new MvcEventEditDetailsViewModel(
                interactorOutput.EventId,
                interactorOutput.OrganisatorName,
                interactorOutput.OrganisatorEmail,
                interactorOutput.OrganisatorAvatarId,
                interactorOutput.StartTime.ToStringSwissDateTime(),
                interactorOutput.EndTime.ToStringSwissDateTime(),
                interactorOutput.Title,
                interactorOutput.Location,
                interactorOutput.MeetingPoint,
                interactorOutput.Description);
        }

        public MvcEventEditDetailsViewModel GetViewModel() => viewModel;
    }
}