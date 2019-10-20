using System;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Administration.AddMemberUseCase
{
    public class AddMember : IRequest<UseCaseResult<string>>
    {
        public AddMember([NotNull] string userName, [NotNull] string firstName, [NotNull] string lastName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        }

        public string UserName { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}