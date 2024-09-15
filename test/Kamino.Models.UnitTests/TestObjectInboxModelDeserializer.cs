namespace Kamino.Models.UnitTests;

[TestClass]
public class TestObjectInboxModelDeserializer
{
    [TestMethod]
    [DynamicData(nameof(ValidFixtures))]
    public void TestObjectInboxModelValid(string fixture)
    {
        var json = File.ReadAllText(fixture);
        var objectModel = JsonSerializer.Deserialize<ObjectInboxModel>(json, DefaultJsonSerializerOptions());

        Assert.IsNotNull(objectModel);
    }

    private static IEnumerable<string[]> ValidFixtures
    {
        get { return GetFixturesForPath("./fixtures/inbox/object/valid"); }
    }

    private static IEnumerable<string[]> GetFixturesForPath(string path)
    {
        foreach (var fixture in Directory.EnumerateFiles(path, "*.json"))
        {
            yield return new string[] { fixture };
        }
    }

    private static JsonSerializerOptions DefaultJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }
}
