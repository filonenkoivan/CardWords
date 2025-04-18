using Api.Models;
using Domain.Models;
using FluentValidation;

namespace Api.Validation
{
    public class CustomValidator : AbstractValidator<UserRequest>
    {
        public CustomValidator()
        {
            RuleFor(x => x.Name.Length).NotNull().GreaterThanOrEqualTo(5).WithMessage("The length of the login must be at least 5 characters.");
            RuleFor(x => x.Password.Length).NotNull().GreaterThanOrEqualTo(5).WithMessage("The length of the password must be at least 5 characters.");
            RuleFor(x => x.PasswordConfirm).NotNull().Matches(x => x.Password).WithMessage("Passwords do not match");
        }
    }
}
