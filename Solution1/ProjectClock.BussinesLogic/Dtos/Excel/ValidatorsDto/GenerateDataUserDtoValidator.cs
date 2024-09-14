using FluentValidation;
using ProjectClock.BusinessLogic.Services.ExcelRaportServices;

namespace ProjectClock.BusinessLogic.Dtos.Excel.ValidatorsDto
{
    public class GenerateDataUserDtoValidator : AbstractValidator<GenerateDataDto>
    {
        public GenerateDataUserDtoValidator()
        {
            RuleFor(d => d.fromDate)
                .LessThanOrEqualTo(d => d.toDate)
                .WithMessage("Date 'From' must be before 'To'");
            RuleFor(d => d.toDate)
               .GreaterThanOrEqualTo(d => d.fromDate)
               .WithMessage("Date 'To' must be after 'Before'");
        }
    }
}
