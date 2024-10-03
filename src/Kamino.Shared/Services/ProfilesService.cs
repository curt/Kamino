using Kamino.Shared.Models;
using Kamino.Shared.Repo;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Shared.Services;

public class ProfilesService(
    IDbContextFactory<NpgsqlContext> contextFactory,
    IdentifierProvider identifierProvider
)
{
    public async Task<ProfileActivityModel> GetPublicProfileAsync()
    {
        using var context = contextFactory.CreateDbContext();
        var profile = await PublicProfileAsync(context);

        return CreateApiModel(profile);
    }

    public async Task<ProfileWebfingerModel> GetPublicProfileByResourceAsync(string resource)
    {
        using var context = contextFactory.CreateDbContext();
        var profile = await PublicProfileAsync(context);
        var name = profile.Name;
        var host = profile.Uri!.Host;

        if (
            !(
                $"acct:{name}".Equals(resource, StringComparison.OrdinalIgnoreCase)
                || $"acct:{name}@{host}".Equals(resource, StringComparison.OrdinalIgnoreCase)
            )
        )
        {
            throw new NotFoundException();
        }

        return CreateWebfingerModel(profile);
    }

    private static ProfileActivityModel CreateApiModel(Profile profile) =>
        new()
        {
            Id = profile.Uri,
            Type = "Person",
            Inbox = new UriBuilder(profile.Uri!) { Path = "/inbox" }.Uri,
            Outbox = new UriBuilder(profile.Uri!) { Path = "/outbox" }.Uri,
            Followers = new UriBuilder(profile.Uri!) { Path = "/followers" }.Uri,
            Following = new UriBuilder(profile.Uri!) { Path = "/following" }.Uri,
            Name = profile.DisplayName,
            PreferredUsername = profile.Name,
            Summary = profile.Summary,
            Url = profile.Url,
            PublicKey = new
            {
                Id = profile.PublicKeyId,
                Owner = profile.Uri,
                PublicKeyPem = profile.PublicKey,
            },
        };

    private static ProfileWebfingerModel CreateWebfingerModel(Profile profile) =>
        new()
        {
            Aliases = [profile.Uri!],
            Links =
            [
                new LinkWebfingerModel()
                {
                    Href = profile.Uri!,
                    Rel = "self",
                    Type = "application/activity+json",
                },
            ],
            Subject = $"acct:{profile.Name}@{profile.Uri!.Host}",
        };

    private async Task<Profile> PublicProfileAsync(Context context)
    {
        return await context
                .Profiles.WhereUriMatch(identifierProvider.GetProfileJson())
                .SingleOrDefaultAsync() ?? throw new NotFoundException();
    }
}
