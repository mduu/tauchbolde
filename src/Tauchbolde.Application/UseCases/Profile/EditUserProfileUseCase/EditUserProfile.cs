using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Profile.EditUserProfileUseCase
{
    public class EditUserProfile : IRequest<UseCaseResult>
    {
        public EditUserProfile(
            Guid userId,
            string fullname,
            string firstname,
            string lastname,
            string slogan,
            string education,
            string experience,
            string mobilePhone,
            string websiteUrl,
            string twitterHandle,
            string facebookId,
            string skypeId)
        {
            UserId = userId;
            Fullname = fullname;
            Firstname = firstname;
            Lastname = lastname;
            Slogan = slogan;
            Education = education;
            Experience = experience;
            MobilePhone = mobilePhone;
            WebsiteUrl = websiteUrl;
            TwitterHandle = twitterHandle;
            FacebookId = facebookId;
            SkypeId = skypeId;
        }
        
        public Guid UserId { get; }
        public string Fullname { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public string Slogan { get; }
        public string Education { get; }
        public string Experience { get; }
        public string MobilePhone { get; }
        public string WebsiteUrl { get; }
        public string TwitterHandle { get; }
        public string FacebookId { get; }
        public string SkypeId { get; }
    }
}