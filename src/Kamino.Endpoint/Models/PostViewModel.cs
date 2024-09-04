using Kamino.Entities;

namespace Kamino.Endpoint.Models;

public class PostViewModel(Post post, Uri endpoint)
{
    public string? Content
    {
        get
        {
            return post.Source;
        }
    }

    public string Uri
    {
        get
        {
            if (post.Uri != null)
            {
                var uri = new UriBuilder(post.Uri);

                if (uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                {
                    uri.Scheme = endpoint.Scheme;
                    uri.Host = endpoint.Host;
                    uri.Port = endpoint.Port;
                }

                return uri.ToString();
            }

            return "/";
        }
    }

    public string? Title { get { return post.Title; } }

    public string? Summary { get { return post.Summary; } }

    public string? PublishedAt { get { return post.PublishedAt?.ToString(); } }

    public string? AuthorName { get { return post.Author?.Name; } }
}
