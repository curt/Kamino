using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentValidation.Results;

namespace Kamino.Shared.Validators.UnitTests;

[TestClass]
public class TestInboundActivityValidator
{
    [TestMethod]
    [DynamicData(nameof(ValidFixtures))]
    public void TestValid(string fixture)
    {
        var valid = ValidateFixture(fixture);

        Assert.IsTrue(valid.IsValid);
    }

    [TestMethod]
    [DynamicData(nameof(InvalidFixtures))]
    public void TestInvalid(string fixture)
    {
        var valid = ValidateFixture(fixture);

        Assert.IsFalse(valid.IsValid);
    }

    [TestMethod]
    [DynamicData(nameof(MalformedFixtures))]
    public void TestMalformed(string fixture)
    {
        var validator = new InboundActivityValidator();
        var node = DeserializeFixture(fixture);

        Assert.IsNotNull(node);
        Assert.IsInstanceOfType(node, typeof(JsonObject));

        var result = validator.Validate((JsonObject)node);

        Assert.IsFalse(result.IsValid);
    }

    private static ValidationResult ValidateFixture(string fixture)
    {
        var validator = new InboundActivityValidator();
        var node = DeserializeFixture(fixture);

        Assert.IsNotNull(node);
        Assert.IsInstanceOfType(node, typeof(JsonObject));

        return validator.Validate((JsonObject)node);
    }

    private static JsonNode? DeserializeFixture(string fixture)
    {
        var json = File.ReadAllText(fixture);

        return JsonSerializer.Deserialize<JsonNode>(json, Helpers.DefaultJsonSerializerOptions());
    }

    private static IEnumerable<string[]> ValidFixtures =>
        Helpers.GetFixturesForPath("./fixtures/inbound/activity/valid");

    private static IEnumerable<string[]> InvalidFixtures =>
        Helpers.GetFixturesForPath("./fixtures/inbound/activity/invalid");

    private static IEnumerable<string[]> MalformedFixtures =>
        Helpers.GetFixturesForPath("./fixtures/inbound/activity/malformed");
}
