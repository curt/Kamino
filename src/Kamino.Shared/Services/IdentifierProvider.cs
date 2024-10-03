using Medo;
using Microsoft.Extensions.Configuration;

namespace Kamino.Shared.Services;

public class IdentifierProvider(IConfiguration configuration)
{
    private readonly Uri _baseUri =
        configuration.GetValue<Uri>("Kamino:BaseUri")
        ?? throw new InvalidOperationException(
            "Must provide configuration value for 'Kamino:BaseUri"
        );

    private Uri? _profileHtml;

    private Uri? _profileJson;

    private Uri? _keyId;

    private Uri? _postsHtml;

    private Uri? _postsJson;

    public Uri GetBase() => _baseUri;

    public Uri GetProfileHtml() =>
        _profileHtml ??= new UriBuilder(GetBase()) { Path = "/index.html" }.Uri;

    public Uri GetProfileJson() =>
        _profileJson ??= new UriBuilder(GetBase()) { Path = "/index.json" }.Uri;

    public Uri GetKeyId() => _keyId ??= new UriBuilder(GetProfileJson()) { Fragment = "#key" }.Uri;

    public Uri GetPostsHtml() =>
        _postsHtml ??= new UriBuilder(GetBase()) { Path = "/posts/index.html" }.Uri;

    public Uri GetPostsJson() =>
        _postsJson ??= new UriBuilder(GetBase()) { Path = "/posts/index.json" }.Uri;

    public Uri GetPostHtml(Uuid7 uuid7) =>
        _postsHtml ??= new UriBuilder(GetBase())
        {
            Path = $"/posts/{uuid7.ToId22String()}.html",
        }.Uri;

    public Uri GetPostJson(Uuid7 uuid7) =>
        _postsJson ??= new UriBuilder(GetBase())
        {
            Path = $"/posts/{uuid7.ToId22String()}.json",
        }.Uri;
}
