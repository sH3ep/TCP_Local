using FluentValidation;

namespace TPC.Api.Model.Validators
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().Matches(@"^[a-zA-Z-']+$").MinimumLength(4).MaximumLength(20)
                .WithMessage("Imie moze byc zlozone z 4 do 20 liter(bez cyfr i znakow specjalnych)");
            RuleFor(u=>u.LastName)
                .NotEmpty().Matches(@"^[a-zA-Z-']+$").MinimumLength(4).MaximumLength(30)
                .WithMessage("Nazwisko moze byc zlozone z 4 do 30 liter(bez cyfr i znakow specjalnych)");
            RuleFor(u => u.Email)
                .NotEmpty().EmailAddress().WithMessage("Bledny format adresu email");
           
        }
    }
}