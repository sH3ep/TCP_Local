using FluentValidation;

namespace TPC.Api.Model.Validators
{
    public class FeatureValidator:AbstractValidator<Feature>
    {
        public FeatureValidator()
        {
            RuleFor(f=>f.Title)
                .NotEmpty().MinimumLength(1).WithMessage("Nazwa musi skladac sie z minimum 1 znaku");
            RuleFor(f=>f.PriorityId)
                .NotEmpty().GreaterThan(0).WithMessage("Id projektu musi byc wieksze niz 0");
            RuleFor(f=>f.AssignedUserId)
                .NotEmpty().GreaterThan(0).WithMessage("Id uzytkownika musi byc wieksze niz 0");
            RuleFor(f=>f.StatusId)
                .NotEmpty().GreaterThan(0).WithMessage("Id statusu musi byc wieksze niz 0");
            RuleFor(f=>f.PriorityId)
                .NotEmpty().GreaterThan(0).WithMessage("Id priorytetu musi byc wieksze niz 0");
        }
    }
}