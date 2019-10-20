using FluentValidation;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Administration.AddMemberUseCase
{
    [UsedImplicitly]
    public class AddMemberValidator : AbstractValidator<AddMember>
    {
        public AddMemberValidator()
        {
            RuleFor(m => m.UserName).NotEmpty();
            RuleFor(m => m.FirstName).NotEmpty();
            RuleFor(m => m.LastName).NotEmpty();
        }
    }
}