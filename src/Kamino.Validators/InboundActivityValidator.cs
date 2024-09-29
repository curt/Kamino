using System.Text.Json.Nodes;
using FluentValidation;

namespace Kamino.Validators;

public class InboundActivityValidator : AbstractJsonNodeValidator<JsonObject>
{
    public static readonly IEnumerable<string> ValidTypes =
    [
        "Create",
        "Follow",
        "Like",
        "Ping",
        "Pong",
        "Undo",
    ];

    public static readonly IEnumerable<string> IntransitiveTypes = ["Ping"];

    public static readonly IEnumerable<string> UndoableTypes = ["Like", "Follow"];

    public InboundActivityValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(obj => obj["type"])
            .Must(BeString)
            .WithMessage("'{PropertyName}' must be a string.")
            .Must(node => ValidTypes.Contains(node!.GetValue<string>()))
            .WithName("type");

        RuleFor(obj => obj["actor"])
            .Must(BeIdentifiable)
            .WithMessage("'{PropertyName}' must be a string or an object.")
            .WithName("actor");

        When(
            obj => !IntransitiveTypes.Contains(GetString(obj["type"])),
            () =>
            {
                RuleFor(obj => obj["object"])
                    .Must(BeIdentifiable)
                    .WithMessage("'{PropertyName}' must be a string or an object.")
                    .WithName("object");
            }
        );

        When(
            obj => GetString(obj["type"]) == "Undo",
            () =>
            {
                RuleFor(obj => obj["object"])
                    .NotNull()
                    .Must(node =>
                        BeIdentifiable(node)
                        || (BeObject(node) && UndoableTypes.Contains(GetString(node!["type"])))
                    )
                    .WithMessage("'{PropertyName}' must represent an undoable type.")
                    .WithName("object");
            }
        );
    }
}
