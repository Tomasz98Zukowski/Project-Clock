using FluentValidation;
using ProjectClock.BusinessLogic.Dtos.AccountDtos;

namespace ProjectClock.BusinessLogic.Dtos.AccountsValidatorsDto
{
    public class EditEmailDtoValidator : AbstractValidator<EditEmailDto>
    {
        public EditEmailDtoValidator()
        {
            RuleFor(c => c.CurrentEmail)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Insert email");
            RuleFor(c => c.NewEmail)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Insert email");
            RuleFor(c => c.NewEmailRepeat)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Insert email");
        }
    }
}
