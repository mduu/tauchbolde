using System;
using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.Administration.GetMassMailDetails
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
