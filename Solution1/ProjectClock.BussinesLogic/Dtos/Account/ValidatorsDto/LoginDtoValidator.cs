using FluentValidation;
using ProjectClock.BusinessLogic.Dtos.AccountDtos;

namespace ProjectClock.BusinessLogic.Dtos.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Insert email");
            RuleFor(c => c.Password)
                        .NotEmpty()
                        .MinimumLength(8).WithMessage("Password must have more than 8 characters");
        }

    }
}
