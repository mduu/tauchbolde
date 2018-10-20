using Tauchbolde.Common.Model;

namespace Tauchbolde.Web.Models.UserProfileModels
{
    public class ReadProfileModel
    {
        public bool AllowEdit { get; set; }
        public Diver Profile { get; set; }
    }
}
