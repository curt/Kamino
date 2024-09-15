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
            var uri = new UriBuilder(uriString);

            if (uri.Scheme == endpoint.Scheme && uri.Host == endpoint.Host && uri.Port == endpoint.Port)
            {
                uri.Scheme = Constants.InternalScheme;
                uri.Host = Constants.InternalHost;
                uri.Port = Constants.InternalPort;
            }

            return uri.Uri.ToString();
        }

        return uriString;
    }
}
