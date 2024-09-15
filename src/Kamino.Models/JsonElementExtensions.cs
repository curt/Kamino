namespace Kamino.Models;

public static class JsonElementExtensions
{
    public static string? GetStringProperty(this JsonElement jsonElement, string propertyName)
    {
        return jsonElement.TryGetProperty(propertyName, out var innerElement)
            && innerElement.ValueKind == JsonValueKind.String ? innerElement.GetString() : null;
    }
}
