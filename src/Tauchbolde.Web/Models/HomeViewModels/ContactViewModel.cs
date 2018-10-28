using System.ComponentModel.DataAnnotations;

namespace Tauchbolde.Web.Models.HomeViewModels
{
    public class ContactViewModel
    {
        [Display(Name = "Dein Name")]
        [Required]
        public string YourName { get; set; }

        [Display(Name = "Deine E-Mail Adresse")]
        [Required]
        public string YourEmail { get; set; }     

        [Display(Name = "Dein Nachricht an uns")]
        [Required]
        public string Text { get; set; }
        
        public int NumberA { get; set; }
        public int NumberB { get; set; }
        
        [Display(Name = "Resultat")]
        [Required]
        public int Result { get; set; }
    }
}
