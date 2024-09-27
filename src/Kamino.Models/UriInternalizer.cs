namespace Kamino.Models;

public class UriInternalizer(Uri endpoint)
{
    public string ExternalHost { get => endpoint.Host; }

    public string? Externalize(string? uriString)
    {
        if (uriString != null)
        {
            var uri = new UriBuilder(uriString);

            if (uri.Host.Equals(Constants.InternalHost, StringComparison.OrdinalIgnoreCase))
            {
                uri.Scheme = endpoint.Scheme;
                uri.Host = endpoint.Host;
                uri.Port = endpoint.Port;
            }

            return uri.Uri.ToString();
        }

        return uriString;
    }

    public string? Internalize(string? uriString)
    {
        if (uriString != null)
        {
            var uri = new Uri(uriString);

            return Internalize(uri)?.ToString();
        }

        return uriString;
    }

    public Uri? Internalize(Uri? uri)
    {
        if (uri != null)
        {
            var builder = new UriBuilder(uri);

            if (builder.Scheme == endpoint.Scheme && builder.Host == endpoint.Host && builder.Port == endpoint.Port)
            {
                builder.Scheme = Constants.InternalScheme;
                builder.Host = Constants.InternalHost;
                builder.Port = Constants.InternalPort;
            }

            uri = builder.Uri;
        }

        return uri;
    }
}
