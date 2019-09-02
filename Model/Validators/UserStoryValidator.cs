using FluentValidation;

namespace TPC.Api.Model.Validators
{
    public class UserStoryValidator:AbstractValidator<UserStory>
    {
        public UserStoryValidator()
        {
            RuleFor(u => u.Title)
                .NotEmpty().MinimumLength(1).WithMessage("Nazwa musi skladac sie z minimum 1 znaku");
            RuleFor(u => u.AssignedUserId)
                .NotEmpty().GreaterThan(0).WithMessage("Id uzytkownika musi byc wieksze niz 0");
            RuleFor(u => u.StatusId)
                .NotEmpty().GreaterThan(0).WithMessage("Id statusu musi byc wieksze niz 0");
            RuleFor(u => u.FeatureId)
                .NotEmpty().GreaterThan(0).WithMessage("Id feature musi byc wieksze niz 0");
            RuleFor(u => u.PriorityId)
                .NotEmpty().GreaterThan(0).WithMessage("Id priorytetu musi byc wieksze niz 0");
        }
    }
}