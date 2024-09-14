using FluentValidation;
using ProjectClock.BusinessLogic.Dtos.WorkingTime.WorkingTimeDtos;

namespace ProjectClock.BusinessLogic.Dtos.WorkingTime.ValidatorsDto
{
    public class UpdateWorkingTimeDtoValidator : AbstractValidator<UpdateWorkingTimeDto>
    {
        public UpdateWorkingTimeDtoValidator()
        {
            RuleFor(wt => wt.StartTime)
                .NotEmpty();
            RuleFor(wt => wt.EndTime)
                .GreaterThan(wt => wt.StartTime)
                .WithMessage("End time must be letter than start time");

        }
    }
}
