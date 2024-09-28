using System.Security.Cryptography;
using FluentValidation;
using SevenKilo.HttpSignatures;

namespace Kamino.Services;

public class SignatureModelValidator : AbstractValidator<SignatureModel>
{
    private static readonly byte[] s_bytes = RandomNumberGenerator.GetBytes(16);

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

    private static bool CanConvertFromBase64String(string s)
    {
        var b = new Span<byte>(s_bytes);
        return Convert.TryFromBase64String(s, b, out int w);
    }
}
