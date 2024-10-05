using Kamino.Shared.Services;
using Microsoft.Extensions.Configuration;

namespace Kamino.Shared.UnitTests;

[TestClass]
public class TestIdentifierProvider
{
    [TestMethod]
    [DataRow("http://localhost:5050")]
    [DataRow("http://localhost")]
    [DataRow("https://localhost")]
    public void TestNormalBaseUri(string baseUri)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?> { { "Kamino:BaseUri", baseUri } }
            )
            .Build();
        var provider = new IdentifierProvider(configuration);

        Assert.AreEqual($"{baseUri}/", provider.GetBase().ToString());
        Assert.AreEqual($"{baseUri}/index.json", provider.GetProfileJson().ToString());
        Assert.AreEqual($"{baseUri}/inbox.json", provider.GetPathJson("inbox").ToString());
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestMissingConfigurationKey()
    {
        var configuration = new ConfigurationBuilder().Build();
        var provider = new IdentifierProvider(configuration);

        Assert.AreEqual($"http://localhost:5050/", provider.GetBase().ToString());
    }
}
