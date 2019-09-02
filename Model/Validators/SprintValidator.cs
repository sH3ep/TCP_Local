using FluentValidation;

namespace TPC.Api.Model.Validators
{
    public class SprintValidator:AbstractValidator<Sprint>
    {
        public SprintValidator()
        {
            RuleFor(s=>s.ProjectId)
                .NotEmpty().GreaterThan(0).WithMessage("Id projektu musi byc wieksze niz 0");
            RuleFor(s=>s.SprintName)
                .NotEmpty().MinimumLength(1).WithMessage("Nazwa musi skladac sie z minimum 1 znaku");
            RuleFor(s => s.StartDate)
                .NotEmpty().LessThan(s => s.EndDate).WithMessage("Data poczatku musi byc wczesniejsza niz data konca");
            RuleFor(s => s.EndDate)
                .NotEmpty().GreaterThan(s => s.StartDate).WithMessage("Data konca musi byc pozniejsza niz data poczatku");
        }
        
    }
}