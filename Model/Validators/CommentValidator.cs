using FluentValidation;

namespace TPC.Api.Model.Validators
{
    public class CommentValidator:AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(c => c.TaskItemId)
                .NotEmpty().GreaterThan(0).WithMessage("Id taska musi byc wieksze niz 0");
            RuleFor(c => c.Text)
                .NotEmpty().MinimumLength(1).WithMessage("Opis musi skladac sie z minimum 1 znaku");
        }
        
    }
}