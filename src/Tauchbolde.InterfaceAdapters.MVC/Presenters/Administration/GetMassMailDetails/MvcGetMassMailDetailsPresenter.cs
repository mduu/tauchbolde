using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Administration.GetMassMailUseCase;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Administration.GetMassMailDetails
{
    public class MvcGetMassMailDetailsPresenter : IGetMassMailDetailsOutputPort
    {
        private MvcMassMailViewModel viewModel;
        
        public void Output([NotNull] GetMassMailDetailsOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));

            viewModel = new MvcMassMailViewModel(
                string.Join(";",
                    interactorOutput.MailRecipients.Select(d =>
                        $"\"{d.Fullname}\"<{d.EmailAddress}>")));
        }

        public MvcMassMailViewModel GetViewModel()
            => viewModel ?? new MvcMassMailViewModel("");
    }
}