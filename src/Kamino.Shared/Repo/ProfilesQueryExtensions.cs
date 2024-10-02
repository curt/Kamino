using Kamino.Entities;

namespace Kamino.Shared.Repo;

public static class ProfilesQueryExtensions
{
    public static IQueryable<Profile> WhereLocal(this IQueryable<Profile> profiles)
    {
        return profiles.WhereUriMatch(Constants.LocalProfileUri);
    }

    public static IQueryable<Profile> WhereUriMatch(this IQueryable<Profile> profiles, string uri)
    {
        return profiles.Where(profile => profile.Uri == uri);
    }
}
