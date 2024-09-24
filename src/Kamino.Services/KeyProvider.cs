using System.Text.Json;
using Kamino.Models;
using SevenKilo.HttpSignatures;

namespace Kamino.Services;

public class KeyProvider(IHttpClientFactory httpClientFactory) : IKeyProvider
{
    private static readonly JsonSerializerOptions s_jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly Dictionary<string, Uri?> _keyOwners = [];

    public KeyModel? Get(string keyId)
    {
        using var httpClient = httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromSeconds(10);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new("application/activity+json"));
        var response = httpClient.GetStringAsync(keyId).Result;
        var model = JsonSerializer.Deserialize<ObjectInboxModel>(response, s_jsonSerializerOptions);
        _keyOwners[keyId] = model?.Id;

        return new KeyModel(model?.PublicKey?.PublicKeyPem ?? string.Empty, null);
    }

    public Uri? GetKeyOwner(string keyId) => _keyOwners[keyId];
}
