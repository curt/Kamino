using FluentValidation.Results;
using Kamino.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kamino.Validators.UnitTests;

[TestClass]
public class TestInboxActivityValidator
{
    [TestMethod]
    [DynamicData(nameof(ValidFixtures))]
    public void TestInboxActivityValid(string fixture)
    {
        var valid = ValidateFixture(fixture);

        Assert.IsTrue(valid.IsValid);
    }

    [TestMethod]
    [DynamicData(nameof(InvalidFixtures))]
    public void TestInboxActivityInvalid(string fixture)
    {
        var valid = ValidateFixture(fixture);

        Assert.IsFalse(valid.IsValid);
    }

    [TestMethod]
    [DynamicData(nameof(MalformedFixtures))]
    public void TestInboxActivityMalformed(string fixture)
    {
        Assert.ThrowsException<JsonException>
        (
            () => { var valid = ValidateFixture(fixture); }
        );
    }

    private static ValidationResult ValidateFixture(string fixture)
    {
        var json = File.ReadAllText(fixture);
        var validator = new ObjectInboxModelValidator();
        var element = JsonSerializer.Deserialize<ObjectInboxModel>(json, DefaultJsonSerializerOptions());

        return validator.Validate(element!);
    }

    private static IEnumerable<string[]> ValidFixtures
    {
        get { return GetFixturesForPath("./fixtures/inbox/activity/valid"); }
    }

    private static IEnumerable<string[]> InvalidFixtures
    {
        get { return GetFixturesForPath("./fixtures/inbox/activity/invalid"); }
    }

    private static IEnumerable<string[]> MalformedFixtures
    {
        get { return GetFixturesForPath("./fixtures/inbox/activity/malformed"); }
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
