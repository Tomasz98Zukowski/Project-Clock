using FluentValidation;
using ProjectClock.BusinessLogic.Dtos.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClock.BusinessLogic.Dtos.AccountsValidatorsDto
{
    public class DeleteAccountDtoValidator : AbstractValidator<DeleteAccountDto>
    {
        public DeleteAccountDtoValidator()
        {
            RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8).WithMessage("Password must have more than 8 characters");
        }
    }
}
