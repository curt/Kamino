using Kamino.Entities;
using Kamino.Models;
using Kamino.Repo;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Services;

public class ProfilesService(Context context)
{
    public async Task<TModel> GetPublicProfileAsync<TModel>(ModelFactoryBase<Profile, TModel> factory)
    {
        var profile = await context.Profiles
            .WhereLocal()
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();

        return factory.Create(profile);
    }
}
