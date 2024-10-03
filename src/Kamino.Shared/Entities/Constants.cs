namespace Kamino.Shared.Entities;

public static class Constants
{
    public static readonly Uri LocalProfileUri = GetLocalProfileUri();

    public const string InternalScheme = "https";
    public const string InternalHost = "localhost";
    public const int InternalPort = -1;

    private static Uri GetLocalProfileUri()
    {
        var uri = new UriBuilder(InternalScheme, InternalHost);

        return uri.Uri;
    }
}
