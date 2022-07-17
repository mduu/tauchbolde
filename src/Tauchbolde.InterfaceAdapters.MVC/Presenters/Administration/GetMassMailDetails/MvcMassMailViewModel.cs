using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Administration.GetMassMailDetails
{
    public class MvcMassMailViewModel
    {
        public MvcMassMailViewModel([NotNull] string tauchboldeEmailReceiver)
        {
            TauchboldeEmailReceiver = tauchboldeEmailReceiver ?? throw new ArgumentNullException(nameof(tauchboldeEmailReceiver));
        }

        public string TauchboldeEmailReceiver { get; }
    }
}
