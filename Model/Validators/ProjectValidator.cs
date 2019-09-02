using FluentValidation;

namespace TPC.Api.Model.Validators
{
    public class ProjectValidator:AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            RuleFor(p=>p.Title)
                .NotEmpty().MinimumLength(1).WithMessage("Nazwa musi skladac sie z minimum 1 znaku");
        }
    }
}