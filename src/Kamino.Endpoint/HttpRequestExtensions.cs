namespace Kamino.Endpoint;

public static class HttpRequestExtensions
{
    public static Uri GetEndpoint(this HttpRequest request)
    {
        return new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port ?? -1).Uri;
    }
}
