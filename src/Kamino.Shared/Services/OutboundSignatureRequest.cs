using System.Net.Http.Headers;
using SevenKilo.HttpSignatures;

namespace Kamino.Shared.Services;

public class OutboundSignatureRequest(
    Uri target,
    HttpMethod method,
    HttpRequestHeaders headers,
    IdentifierProvider identifierProvider
) : ISignatureRequest
{
    public string KeyId => identifierProvider.GetKeyId().ToString();

    public IEnumerable<string> Headers => ["(request-target)", "host", "date", "digest"];

    public string GetHeaderValue(string key)
    {
        return key switch
        {
            "(request-target)" => $"{method.Method.ToLower()} {target.PathAndQuery}",
            "host" => target.Host,
            _ => headers
                .Where(h => h.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                ?.SelectMany(h => h.Value)
                .FirstOrDefault() ?? string.Empty,
        };
    }
}
