using FluentValidation;
using ProjectClock.BusinessLogic.Dtos.AccountDtos;

namespace ProjectClock.BusinessLogic.Dtos.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty()
                .WithMessage("Insert name")
                .MinimumLength(2)
                .WithMessage("Name must have more than 2 characters");

            RuleFor(c => c.LastName)
                .NotEmpty()
                .WithMessage("Insert surname")
                .MinimumLength(2)
                .WithMessage("Name must have more than 2 characters");

            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Insert email");

            RuleFor(c => c.Password)
                        .NotEmpty()
                        .MinimumLength(8)
                        .WithMessage("Password must have more than 8 characters");

            RuleFor(c => c.ConfirmPassword)
                        .NotEmpty()
                        .Equal(e => e.Password)
                        .WithMessage("You passwords aren't equal")
                        .MinimumLength(8)
                        .WithMessage("Password must have more than 8 characters");

        }

    }
}
