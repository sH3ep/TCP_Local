using FluentValidation;

namespace TPC.Api.Model.Validators
{
    public class TaskItemValidator : AbstractValidator<TaskItem>
    {
        public TaskItemValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty().MinimumLength(1).WithMessage("Nazwa musi skladac sie z minimum 1 znaku");
            RuleFor(t => t.SprintId)
                .NotEmpty().GreaterThan(0).WithMessage("Id sprintu musi byc większe niż 0");
            RuleFor(t => t.UserStoryId)
                .NotEmpty().GreaterThan(0).WithMessage("Id userStory musi byc większe niż 0");
            RuleFor(t => t.AssignedUserId)
                .NotEmpty().GreaterThan(0).WithMessage("Id uzytkownika musi byc wieksze niz 0");
            RuleFor(t=>t.StatusId)
                .NotEmpty().GreaterThan(0).WithMessage("Id statusu musi byc wieksze niz 0");
            RuleFor(t=>t.TaskTypeId)
                .NotEmpty().GreaterThan(0).WithMessage("Id typu musi byc wieksze niz 0");
            RuleFor(t=>t.PriorityId)
                .NotEmpty().GreaterThan(0).WithMessage("Id priorytetu musi byc wieksze niz 0");
        }
    }
}
