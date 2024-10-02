using System.Text.Json.Nodes;
using FluentValidation;

namespace Kamino.Shared.Validators;

public class InboundActorValidator : AbstractJsonNodeValidator<JsonObject>
{
    public InboundActorValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(obj => obj["id"])
            .NotNull()
            .Must(BeString)
            .WithMessage("'{PropertyName}' must be a string.")
            .WithName("id");

        RuleFor(obj => obj["inbox"])
            .NotNull()
            .Must(BeString)
            .WithMessage("'{PropertyName}' must be a string.")
            .WithName("inbox");

        RuleFor(obj => obj["publicKey"])
            .NotNull()
            .Must(BeObject)
            .WithMessage("'{PropertyName}' must be an object.")
            .SetValidator(new InboundPublicKeyValidator())
            .WithName("publicKey");
    }
}
