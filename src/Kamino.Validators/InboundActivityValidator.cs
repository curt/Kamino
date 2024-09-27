using System.Text.Json.Nodes;
using FluentValidation;

namespace Kamino.Validators;

public class InboundActivityValidator : AbstractJsonNodeValidator<JsonObject>
{
    public static readonly IEnumerable<string> ValidTypes = ["Create", "Like", "Ping", "Pong"];

    public static readonly IEnumerable<string> IntransitiveTypes = [];

    public InboundActivityValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(obj => obj["type"])
            .NotNull()
            .Must(BeString)
            .WithMessage("'{PropertyName}' must be a string.")
            .Must(node => ValidTypes.Contains(node!.GetValue<string>()))
            .WithName("type");

        RuleFor(obj => obj["actor"])
            .NotNull()
            .Must(BeStringOrObject)
            .WithMessage("'{PropertyName}' must be a string or an object.")
            .WithName("actor");

        RuleFor(obj => obj["object"])
            .NotNull()
            .Must(BeStringOrObject)
            .WithMessage("'{PropertyName}' must be a string or an object.")
            .WithName("object")
            .When(obj =>
                obj["type"] != null && !IntransitiveTypes.Contains(obj["type"]!.ToString())
            );
    }
}
