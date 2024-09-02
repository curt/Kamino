using Kamino.Entities;

namespace Kamino.Services;

public interface IPostsService
{
    Task<IEnumerable<Post>> GetPublicPostsAsync();
    Task<Post> GetPublicPostByIdAsync(Guid id);
}
