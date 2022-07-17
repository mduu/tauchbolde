namespace Tauchbolde.Application.UseCases.Profile.GetUserProfileUseCase
{
    public class GetUserProfileOutput
    {
        public GetUserProfileOutput(
            bool allowEdit,
            Guid userId,
            IEnumerable<string> roles,
            string email,
            string avatarId,
            string realname,
            string firstname,
            string lastname,
            DateTime? memberSince,
            string slogan,
            string education,
            string experience,
            string mobilePhone,
            string websiteUrl,
            string twitterHandle,
            string facebookId,
            string skypeId)
        {
            AllowEdit = allowEdit;
            UserId = userId;
            Roles = roles;
            Email = email;
            AvatarId = avatarId;
            Realname = realname;
            Firstname = firstname;
            Lastname = lastname;
            MemberSince = memberSince;
            Slogan = slogan;
            Education = education;
            Experience = experience;
            MobilePhone = mobilePhone;
            WebsiteUrl = websiteUrl;
            TwitterHandle = twitterHandle;
            FacebookId = facebookId;
            SkypeId = skypeId;
        }

        public bool AllowEdit { get; }
        public Guid UserId { get; }
        public IEnumerable<string> Roles { get; }
        public string Email { get; }
        public string AvatarId { get; }
        public string Realname { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public DateTime? MemberSince { get; }
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