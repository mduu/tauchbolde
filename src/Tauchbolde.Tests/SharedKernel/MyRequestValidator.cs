using FluentValidation;
using JetBrains.Annotations;

namespace Tauchbolde.Tests.SharedKernel
{
    [UsedImplicitly]
    public class MyRequestValidator : AbstractValidator<MyRequest>
    {
        public MyRequestValidator()
        {
            RuleFor(r => r.Number).InclusiveBetween(2, 5);
        }
    }
}