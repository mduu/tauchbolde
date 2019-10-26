using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Tauchbolde.InterfaceAdapters.Administration.EditRoles
{
    public class MvcEditRolesViewModel
    {
        public MvcEditRolesViewModel(
            [NotNull] string userName,
            [NotNull] string fullName,
            [NotNull] IEnumerable<string> allRoles,
            [NotNull] IEnumerable<string> assignedRoles)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            AllRoles = allRoles ?? throw new ArgumentNullException(nameof(allRoles));
            AssignedRoles = assignedRoles ?? throw new ArgumentNullException(nameof(assignedRoles));
        }

        [NotNull] public string UserName { get; }
        [NotNull] public string FullName { get; }
        [NotNull] public IEnumerable<string> AllRoles { get; }
        [NotNull] public IEnumerable<string> AssignedRoles { get; }
    }
}