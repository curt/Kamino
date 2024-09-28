using System.Net.Http.Headers;
using Kamino.Entities;
using Kamino.Models;
using SevenKilo.HttpSignatures;

namespace Kamino.Services;

public class OutboundSignatureRequest(
    Uri target,
    HttpMethod method,
    HttpContentHeaders headers,
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
            "(request-target)" => $"{method.ToString().ToLower()} {target.PathAndQuery}",
            _ => headers
                .Where(h => h.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                ?.SelectMany(h => h.Value)
                .FirstOrDefault() ?? string.Empty,
        };
    }
}
