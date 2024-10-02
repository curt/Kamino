using FluentValidation;
using SevenKilo.HttpSignatures;

namespace Kamino.Shared.Services;

public class SignatureModelValidator : AbstractValidator<SignatureModel>
{
    public SignatureModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(a => a.KeyId).NotEmpty();

        RuleFor(a => a.SignatureHash).NotEmpty();

        RuleFor(a => a.Headers)
            .Must(h => h.Contains("digest", StringComparer.OrdinalIgnoreCase))
            .WithMessage("{PropertyName} must contain the 'digest' value.");

        RuleFor(a => a.Headers)
            .Must(h =>
                h.Contains("date", StringComparer.OrdinalIgnoreCase)
                || h.Contains("(created)", StringComparer.OrdinalIgnoreCase)
            )
            .WithMessage("{PropertyName} must contain either the 'date' or '(created)' value.");
    }
}
