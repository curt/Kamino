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

    private void AfterAddPost(Post post)
    {
        var uri = PostUriFromGuid(post.Id);

        post.Uri = uri;
        post.Url = uri;
    }

    private static string? PostUriFromGuid(Guid? guid)
    {
        if (guid == null)
        {
            return null;
        }

        var uri = new UriBuilder(Constants.LocalProfileUri)
        {
            Path = $"/p/{guid.Value.ToId22()}"
        };

        return uri.Uri.ToString();
    }
}
