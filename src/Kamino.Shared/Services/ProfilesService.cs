using Kamino.Entities;
using Kamino.Models;
using Kamino.Repo;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Shared.Services;

public class ProfilesService(Context context)
{
    public async Task<TModel> GetPublicProfileAsync<TModel>(
        ModelFactoryBase<Profile, TModel> factory
    )
    {
        var profile = await PublicProfileAsync();

        return factory.Create(profile);
    }

    public async Task<TModel> GetPublicProfileByResourceAsync<TModel>(
        string resource,
        ModelFactoryBase<Profile, TModel> factory
    )
    {
        var profile = await PublicProfileAsync();
        var name = profile.Name;
        var host = factory.UriInternalizer.ExternalHost;

        if (
            !(
                $"acct:{name}".Equals(resource, StringComparison.OrdinalIgnoreCase)
                || $"acct:{name}@{host}".Equals(resource, StringComparison.OrdinalIgnoreCase)
            )
        )
        {
            throw new NotFoundException();
        }

        return factory.Create(profile);
    }

    private async Task<Profile> PublicProfileAsync()
    {
        return await context.Profiles.WhereLocal().SingleOrDefaultAsync()
            ?? throw new NotFoundException();
    }
}
