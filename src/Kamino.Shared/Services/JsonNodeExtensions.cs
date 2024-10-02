using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kamino.Shared.Services;

public static class JsonNodeExtensions
{
    public static string? GetString(this JsonNode? node) =>
        node != null && node.GetValueKind() == JsonValueKind.String
            ? node.GetValue<string>()
            : null;

    public static string? GetString(this JsonNode? node, string property) =>
        node?[property].GetString();

    public static Uri? GetUri(this JsonNode? node)
    {
        var str = node.GetString();
        return str != null ? new Uri(str) : null;
    }

    public static Uri? GetUri(this JsonNode? node, string property) => node?[property].GetUri();
}
