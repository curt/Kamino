using System.Text.Json.Nodes;
using FluentValidation;

namespace Kamino.Shared.Validators;

public class InboundPublicKeyValidator : AbstractJsonNodeValidator<JsonNode?>
{
    public InboundPublicKeyValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(obj => obj!["id"])
            .NotNull()
            .Must(BeString)
            .WithMessage("'{PropertyName}' must be a string.")
            .WithName("id");

        RuleFor(obj => obj!["owner"])
            .NotNull()
            .Must(BeString)
            .WithMessage("'{PropertyName}' must be a string.")
            .WithName("owner");

        RuleFor(obj => obj!["publicKeyPem"])
            .NotNull()
            .Must(BeString)
            .WithMessage("'{PropertyName}' must be a string.")
            .WithName("publicKeyPem");
    }
}
