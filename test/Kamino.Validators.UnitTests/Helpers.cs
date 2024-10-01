using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kamino.Shared.Validators.UnitTests;

public class Helpers
{
    public static IEnumerable<string[]> GetFixturesForPath(string path)
    {
        foreach (var fixture in Directory.EnumerateFiles(path, "*.json"))
        {
            yield return new string[] { fixture };
        }
    }

    public static JsonSerializerOptions DefaultJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
        };
    }
}
