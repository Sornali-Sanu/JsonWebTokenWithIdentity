using FluentValidation;
using JsonWebTokenwithIdentity.Extensions;
using JsonWebTokenwithIdentity.Models.ViewModels;

namespace JsonWebTokenwithIdentity.Validation
{
    public class RegisterRequestValidator:AbstractValidator<RegisterViewModel>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Password).Password();
        }
    }
}
