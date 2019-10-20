using FluentValidation;
using JetBrains.Annotations;

namespace Tauchbolde.Application.UseCases.Administration.EditRolesUseCase
{
    [UsedImplicitly]
    internal class EditRolesValidator : AbstractValidator<EditRoles>
    {
        public EditRolesValidator()
        {
            RuleFor(m => m.UserName).NotEmpty();
            RuleFor(m => m.Roles).NotNull();
        }
    }
}