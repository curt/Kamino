using System.Text.Json;

namespace Kamino.Validators.UnitTests;

[TestClass]
public class TestInboxActivityValidator
{
    [TestMethod]
    [DynamicData(nameof(ValidFixtures))]
    public void TestInboxActivityValid(string fixture)
    {
        var json = File.ReadAllText(fixture);
        var validator = new InboxActivityValidator();
        var element = DeserializeJsonElement(json);
        var valid = validator.Validate(element);

        Assert.IsTrue(valid.IsValid);
    }

    [TestMethod]
    [DynamicData(nameof(InvalidFixtures))]
    public void TestInboxActivityInvalid(string fixture)
    {
        var json = File.ReadAllText(fixture);
        var validator = new InboxActivityValidator();
        var element = DeserializeJsonElement(json);
        var valid = validator.Validate(element);

        Assert.IsFalse(valid.IsValid);
    }

    private static IEnumerable<string[]> ValidFixtures
    {
        get { return GetFixturesForPath("./fixtures/inbox/activity/valid"); }
    }

    private static IEnumerable<string[]> InvalidFixtures
    {
        get { return GetFixturesForPath("./fixtures/inbox/activity/invalid"); }
    }

    private static IEnumerable<string[]> GetFixturesForPath(string path)
    {
        foreach (var fixture in Directory.EnumerateFiles(path, "*.json"))
        {
            yield return new string[] { fixture };
        }
    }

    private JsonElement DeserializeJsonElement(string json)
    {
        return JsonDocument.Parse(json).RootElement;
    }
}
