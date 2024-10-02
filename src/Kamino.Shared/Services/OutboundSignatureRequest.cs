using System.Net.Http.Headers;
using Kamino.Entities;
using Kamino.Shared.Models;
using SevenKilo.HttpSignatures;

namespace Kamino.Shared.Services;

public class OutboundSignatureRequest(
    Uri target,
    HttpMethod method,
    HttpRequestHeaders headers,
    Uri endpoint
) : ISignatureRequest
{
    public string KeyId =>
        new UriInternalizer(endpoint).Externalize(Constants.LocalProfileUri + "#key")!;

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
