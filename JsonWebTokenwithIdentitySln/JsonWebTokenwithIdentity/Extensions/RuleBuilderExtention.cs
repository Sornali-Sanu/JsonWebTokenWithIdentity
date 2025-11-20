using FluentValidation;

namespace JsonWebTokenwithIdentity.Extensions
{
    public static class RuleBuilderExtention
    {
        public static void Password<T>(this IRuleBuilder<T, string> ruleBuilder,int minimumLength=8)
        {
            ruleBuilder.MinimumLength(minimumLength).WithMessage($"minimum length is not satisfied {minimumLength}")
                .Matches("[a-z]").WithMessage("you need at least one lowercase letter")
                .Matches("[A-Z]").WithMessage("you need at least one uppercase letter")
                .Matches("[0-9]").WithMessage("you need at least one digit")
                .Matches("[^a-zA-Z0-9]").WithMessage("you need at least one special character");
        }
    }
}
