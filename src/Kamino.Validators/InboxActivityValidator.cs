using System.Text.Json;
using FluentValidation;

namespace Kamino.Validators;

public class InboxActivityValidator : AbstractValidator<JsonElement>
{
    public InboxActivityValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(a => GetJsonProperty(a, "type"))
            .NotNull()
            .Must(a => a?.ValueKind == JsonValueKind.String)
            .WithName("type");

        RuleFor(a => GetJsonProperty(a, "actor"))
            .NotNull()
            .WithName("actor");

        RuleFor(a => GetJsonProperty(a, "object"))
            .NotNull()
            .WithName("object");
    }

    protected JsonElement? GetJsonProperty(JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var result))
        {
            return result;
        }

        return null;
    }
}
