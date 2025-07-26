using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]+").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]+").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
            .Matches(@"[0-9]+").WithMessage("A senha deve conter pelo menos um número.")
            .Matches(@"[\!\?\*\.\@\#\$\%\^\&\+\=]+").WithMessage("A senha deve conter pelo menos um caractere especial.");
    }
}