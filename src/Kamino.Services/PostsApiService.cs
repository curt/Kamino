using Kamino.Entities;
using Kamino.Models;
using Kamino.Repo;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Services;

public class PostsApiService(Context context, Uri endpoint)
{
    public async Task<IEnumerable<PostApiModel>> GetPostsAsync()
    {
        var posts = await context.Posts
            .Include(post => post.Author)
            .Include(post => post.Places)
            .Include(post => post.Tags)
            .ToListAsync();

        var factory = new PostApiModelFactory(endpoint);
        var models = posts.Select(post => factory.Create(post));

        return models;
    }

    public async Task<PostApiModel> PostPostAsync(PostApiModel model)
    {
        var factory = new PostApiModelFactory(endpoint);
        Post post = factory.Parse(model);

        var authorUri = factory.UriInternalizer.Internalize(model.AuthorUri!);
        post.Author = await SingleProfileAsync(authorUri!);

        context.Add(post);
        AfterAddPost(post);
        context.SaveChanges();

        return factory.Create(post);
    }

    private async Task<Profile> SingleProfileAsync(string uri)
    {
        return await context.Profiles
            .WhereLocal()
            .Where(profile => profile.Uri == uri)
            .SingleOrDefaultAsync() ?? throw new BadRequestException();
    }

    private static void AfterAddPost(Post post)
    {
        if (post.Id != null)
        {
            var guid = post.Id.Value;

            var uri = PostUriFromGuid(guid);
            post.Uri = uri;
            post.Url = uri;

            var contextUri = ContextUriFromGuid(guid);
            post.ContextUri ??= contextUri;
        }
    }

    private static string PostUriFromGuid(Guid guid) => UriFromGuid(guid, "/p/{0}");

    private static string ContextUriFromGuid(Guid guid) => UriFromGuid(guid, "/ctx/{0}");

    private static string UriFromGuid(Guid guid, string format)
    {
        var uri = new UriBuilder(Constants.LocalProfileUri)
        {
            Path = string.Format(format, guid.ToId22())
        };

        return uri.Uri.ToString();
    }
}
