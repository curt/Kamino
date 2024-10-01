using System.Text.Json;
using System.Text.Json.Nodes;
using Kamino.Shared.Validators;
using Microsoft.Extensions.Logging;
using SevenKilo.HttpSignatures;

namespace Kamino.Services;

public class KeyProvider(ILogger logger, IHttpClientFactory httpClientFactory) : IKeyProvider
{
    private static readonly JsonSerializerOptions s_jsonSerializerOptions =
        new(JsonSerializerDefaults.Web);

    private Uri? _owner = null;
    private JsonObject? _actor = null;

    public Uri? Owner => _owner;

    public JsonObject? Actor => _actor;

    public async Task<KeyModel?> GetKeyModelByKeyIdAsync(string keyId)
    {
        using var httpClient = httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromSeconds(10);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new("application/activity+json"));

        try
        {
            logger.LogInformation("Fetching actor for key {keyId}.", keyId);

            var response = await httpClient.GetStringAsync(keyId);
            _actor = JsonSerializer.Deserialize<JsonObject>(response, s_jsonSerializerOptions);

            if (_actor != null)
            {
                var validator = new InboundActorValidator();
                var result = validator.Validate(_actor);

                if (result.IsValid)
                {
                    var modelId = _actor["id"]!.ToString();
                    _owner = new(modelId);

                    return new KeyModel(_actor["publicKey"]!["publicKeyPem"]!.ToString(), null);
                }
                else
                {
                    logger.LogWarning(
                        "Inbound actor failed validation for key '{keyId}': {errors}.",
                        keyId,
                        string.Join(' ', result.Errors)
                    );
                }
            }
            else
            {
                logger.LogWarning("Inbound actor failed deserialization for key '{keyId}'.", keyId);
            }
        }
        catch (HttpRequestException httpRequestException)
        {
            logger.LogWarning(
                httpRequestException,
                "Exception while retrieving actor for key '{keyId}'.",
                keyId
            );
        }
        catch (JsonException jsonException)
        {
            logger.LogWarning(
                jsonException,
                "Exception while deserializing actor for key '{keyId}'.",
                keyId
            );
        }

        return null;
    }
}
