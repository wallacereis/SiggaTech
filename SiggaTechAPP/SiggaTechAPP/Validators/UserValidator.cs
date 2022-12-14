using FluentValidation;
using SiggaTechAPP.Models;
using System.Text.RegularExpressions;

namespace SiggaTechAPP.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Por favor, informe o E-mail!")
                .Matches(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase)
                .WithMessage("Por favor, informe um E-mail válido!");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Por favor, informe a Senha!")
                .MinimumLength(3).WithMessage("A Senha deve ter no mínimo 3 caracteres")
                .MaximumLength(30).WithMessage("A Senha deve ter no máximo 30 caracteres");
        }
    }
}
