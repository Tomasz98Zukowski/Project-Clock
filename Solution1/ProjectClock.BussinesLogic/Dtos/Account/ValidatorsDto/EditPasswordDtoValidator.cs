using FluentValidation;
using ProjectClock.BusinessLogic.Dtos.AccountDtos;

namespace ProjectClock.BusinessLogic.Dtos.AccountsValidatorsDto
{
    public class EditPasswordDtoValidator : AbstractValidator<EditPasswordDto>
    {
        public EditPasswordDtoValidator()
        {
            RuleFor(c => c.CurrentPassword)
                      .NotEmpty()
                      .MinimumLength(8).WithMessage("Password must have more than 8 characters");
            RuleFor(c => c.NewPassword)
                       .NotEmpty()
                       .MinimumLength(8).WithMessage("Password must have more than 8 characters");
            RuleFor(c => c.ConfirmNewPassword)
                        .NotEmpty()
                        .Equal(e => e.NewPassword)
                        .WithMessage("You passwords aren't equal");
        }
    }
}
