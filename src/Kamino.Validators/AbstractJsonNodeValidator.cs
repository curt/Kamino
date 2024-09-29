using System.Text.Json;
using System.Text.Json.Nodes;
using FluentValidation;

namespace Kamino.Validators;

public abstract class AbstractJsonNodeValidator<T> : AbstractValidator<T>
    where T : JsonNode?
{
    protected static bool BeString(JsonNode? node) =>
        node != null && BeOfKind(node, JsonValueKind.String);

    protected static bool BeObject(JsonNode? node) =>
        node != null && BeOfKind(node, JsonValueKind.Object);

    protected static bool BeStringOrObject(JsonNode? node) => BeString(node) || BeObject(node);

    protected static bool BeIdentifiable(JsonNode? node) =>
        BeString(node) || (BeObject(node) && (BeString(node!["id"]) || BeString(node!["href"])));

    protected static string GetString(JsonNode? node) =>
        BeString(node) ? node!.ToString() : string.Empty;

    private static bool BeOfKind(JsonNode node, JsonValueKind kind) => node.GetValueKind() == kind;
}
