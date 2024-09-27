using FluentValidation;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kamino.Validators;

public abstract class AbstractJsonNodeValidator<T> : AbstractValidator<T> where T : JsonNode?
{
    protected static bool BeString(JsonNode? node) => BeOfKind(node!, JsonValueKind.String);

    protected static bool BeObject(JsonNode? node) => BeOfKind(node!, JsonValueKind.Object);

    protected static bool BeStringOrObject(JsonNode? node) => BeString(node) || BeObject(node);

    private static bool BeOfKind(JsonNode node, JsonValueKind kind) => node.GetValueKind() == kind;
}
