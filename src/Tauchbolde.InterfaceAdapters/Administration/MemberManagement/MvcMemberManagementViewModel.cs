using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.Administration.MemberManagement
{
    public class MvcMemberManagementViewModel
    {
        public MvcMemberManagementViewModel(
            [NotNull] IEnumerable<MemberViewModel> members,
            [NotNull] IEnumerable<string> addableUsers)
        {
            Members = members ?? throw new ArgumentNullException(nameof(members));
            AddableUsers = addableUsers ?? throw new ArgumentNullException(nameof(addableUsers));
        }

        [NotNull] public IEnumerable<MemberViewModel> Members { get; }
        [NotNull] public IEnumerable<string> AddableUsers { get; }

        public class MemberViewModel
        {
            public MemberViewModel(
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