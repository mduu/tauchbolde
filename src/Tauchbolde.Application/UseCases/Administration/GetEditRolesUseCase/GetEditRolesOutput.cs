using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Administration.GetEditRolesUseCase
{
    public class GetEditRolesOutput
    {
        public GetEditRolesOutput(
            [NotNull] string userName,
            [NotNull] string fullName,
            IEnumerable<string> allRoles,
            IEnumerable<string> assignedRoles)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            AllRoles = allRoles ?? Enumerable.Empty<string>();
            AssignedRoles = assignedRoles ?? Enumerable.Empty<string>();
        }

        [NotNull] public string UserName { get; }
        [NotNull] public string FullName { get; }
        [NotNull] public IEnumerable<string> AllRoles { get; }
        [NotNull] public IEnumerable<string> AssignedRoles { get; }
    }
}