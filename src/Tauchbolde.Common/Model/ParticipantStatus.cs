using System.ComponentModel.DataAnnotations;

namespace Tauchbolde.Common.Model
{
    public enum ParticipantStatus
    {
        [Display(Name = "Unbekannt")]
        None,
        [Display(Name = "Bin dabei")]
        Accepted,
        [Display(Name = "Kann nicht")]
        Declined,
        [Display(Name = "Noch unsicher")]
        Tentative,
    }
}