using FluentValidation;
using TPC.Api.Model.Dto;

namespace TPC.Api.Model.Validators
{
    public class UserDtoValidator:AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().Matches(@"^[a-zA-Z-']+$").MinimumLength(4).MaximumLength(20)
                .WithMessage("Imie moze byc zlozone z 4 do 20 liter(bez cyfr i znakow specjalnych)");
            RuleFor(u => u.LastName)
                .NotEmpty().Matches(@"^[a-zA-Z-']+$").MinimumLength(4).MaximumLength(30)
                .WithMessage("Nazwisko moze byc zlozone z 4 do 30 liter(bez cyfr i znakow specjalnych)");
        }
    }
}