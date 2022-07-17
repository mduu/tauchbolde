using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Administration.GetMemberManagementUseCase
{
    public class MemberManagementOutput
    {
        public MemberManagementOutput(
            [NotNull] IEnumerable<Member> members,
            [NotNull] IEnumerable<string> addableUsers)
        {
            Members = members ?? throw new ArgumentNullException(nameof(members));
            AddableUsers = addableUsers ?? throw new ArgumentNullException(nameof(addableUsers));
        }

        [NotNull] public IEnumerable<Member> Members { get; }
        [NotNull] public IEnumerable<string> AddableUsers { get; }

        public class Member
        {
            public Member(
                Guid diverId,
                string fullname,
                [NotNull] string userName,
                [NotNull] string email,
                bool emailConfirmed,
                bool lockoutEnabled,
                [NotNull] IEnumerable<string> roles)
            {
                DiverId = diverId;
                Fullname = fullname;
                UserName = userName ?? throw new ArgumentNullException(nameof(userName));
                Email = email ?? throw new ArgumentNullException(nameof(email));
                EmailConfirmed = emailConfirmed;
                LockoutEnabled = lockoutEnabled;
                Roles = roles ?? throw new ArgumentNullException(nameof(roles));
            }

            public Guid DiverId { get; }
            public string Fullname { get; }
            [NotNull] public string UserName { get; }
            [NotNull] public string Email { get; }
            public bool EmailConfirmed { get; }
            public bool LockoutEnabled { get; }
            [NotNull] public IEnumerable<string> Roles { get; }
        }
    }
}