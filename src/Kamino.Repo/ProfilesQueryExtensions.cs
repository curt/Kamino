using Kamino.Entities;

namespace Kamino.Repo;

public static class ProfilesQueryExtensions
{
    public static IQueryable<Profile> WhereUriMatch(this IQueryable<Profile> profiles, string uri)
    {
        return profiles.Where(profile => profile.Uri == uri);
    }
}
