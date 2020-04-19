using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Profile.MemberListUseCase
{
    public class MemberListOutput
    {
        public MemberListOutput(
            bool allowSeeDetails,
            [CanBeNull] IEnumerable<Member> members)
        {
            AllowSeeDetails = allowSeeDetails;
            Members = members ?? Enumerable.Empty<Member>();
        }

        public bool AllowSeeDetails { get; }
        public IEnumerable<Member> Members { get; }

        public class Member
        {
            public Member(
                Guid diverId,
                [NotNull] string email,
                [CanBeNull] string avatarId,
                [NotNull] string name,
                DateTime memberSince,
                string education,
                string experience,
                string slogan,
                string websiteUrl,
                string twitterHandle,
                string facebookId)
            {
                DiverId = diverId;
                Email = email ?? throw new ArgumentNullException(nameof(email));
                AvatarId = avatarId;
                Name = name ?? throw new ArgumentNullException(nameof(name));
                Education = education;
                Experience = experience;
                Slogan = slogan;
                MemberSince = memberSince;
                TwitterHandle = twitterHandle;
                FacebookId = facebookId;
                WebsiteUrl = websiteUrl;
            }

            public Guid DiverId { get; }
            [NotNull] public string Email { get; }
            [CanBeNull] public string AvatarId { get; }
            [NotNull] public string Name { get; }
            public DateTime MemberSince { get; }
            public string Education { get; }
            public string Experience { get; }
            public string Slogan { get; }
            public string WebsiteUrl { get; }
            public string TwitterHandle { get; }
            public string FacebookId { get; }
        }
    }
}