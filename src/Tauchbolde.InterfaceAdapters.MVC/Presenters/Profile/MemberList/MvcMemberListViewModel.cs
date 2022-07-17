using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.MVC.Presenters.Profile.MemberList
{
    public class MvcMemberListViewModel
    {
        public MvcMemberListViewModel(bool allowSeeDetails, IEnumerable<MvcMemberViewModel> members)
        {
            AllowSeeDetails = allowSeeDetails;
            Members = members ?? Enumerable.Empty<MvcMemberViewModel>();
        }

        public bool AllowSeeDetails { get; }
        public IEnumerable<MvcMemberViewModel> Members { get; }

        public class MvcMemberViewModel
        {
            public MvcMemberViewModel(
                Guid diverId,
                [NotNull] string email,
                [CanBeNull] string avatarId,
                [NotNull] string name,
                [NotNull] string memberSince,
                string education,
                string experience,
                string slogan,
                string websiteUrl,
                string twitterHandle,
                string twitterUrl,
                string facebookId,
                string facebookUrl)
            {
                DiverId = diverId;
                Email = email ?? throw new ArgumentNullException(nameof(email));
                AvatarId = avatarId;
                Name = name ?? throw new ArgumentNullException(nameof(name));
                MemberSince = memberSince ?? throw new ArgumentNullException(nameof(memberSince));
                Education = education;
                Experience = experience;
                Slogan = slogan;
                TwitterHandle = twitterHandle;
                TwitterUrl = twitterUrl;
                FacebookId = facebookId;
                FacebookUrl = facebookUrl;
                WebsiteUrl = websiteUrl;
            }

            public Guid DiverId { get; }
            [NotNull] public string Email { get; }
            [NotNull] public string AvatarId { get; }
            [NotNull] public string Name { get; }
            public string MemberSince { get; }
            public string Education { get; }
            public string Experience { get; }
            public string Slogan { get; }
            public string WebsiteUrl { get; }
            public string TwitterHandle { get; }
            public string TwitterUrl { get; }
            public string FacebookId { get; }
            public string FacebookUrl { get; }
        }
    }
}