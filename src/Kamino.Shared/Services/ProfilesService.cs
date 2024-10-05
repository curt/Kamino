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

        return CreatePublicActivityModel(profile);
    }

    public async Task<ProfileWebfingerModel> GetPublicProfileByResourceAsync(string resource)
    {
        using var context = contextFactory.CreateDbContext();
        var profile = await PublicProfileAsync(context);
        var name = profile.Name;
        var host = identifierProvider.GetBase().Host;

        if (
            !(
                $"acct:{name}".Equals(resource, StringComparison.OrdinalIgnoreCase)
                || $"acct:{name}@{host}".Equals(resource, StringComparison.OrdinalIgnoreCase)
            )
        )
        {
            throw new NotFoundException();
        }

        return CreatePublicWebfingerModel(profile);
    }

    private ProfileActivityModel CreatePublicActivityModel(Profile profile) =>
        new()
        {
            Id = identifierProvider.GetProfileJson().ToString(),
            Type = "Person",
            Inbox = identifierProvider.GetPathJson("inbox").ToString(),
            Outbox = identifierProvider.GetPathJson("outbox").ToString(),
            Followers = identifierProvider.GetPathJson("followers").ToString(),
            Following = identifierProvider.GetPathJson("following").ToString(),
            Name = profile.DisplayName,
            PreferredUsername = profile.Name,
            Summary = profile.Summary,
            Url = identifierProvider.GetProfileHtml().ToString(),
            PublicKey = new PublicKeyActivityModel
            {
                Id = identifierProvider.GetKeyId().ToString(),
                Owner = identifierProvider.GetProfileJson().ToString(),
                PublicKeyPem = profile.PublicKey,
            },
        };

    private ProfileWebfingerModel CreatePublicWebfingerModel(Profile profile) =>
        new()
        {
            Aliases = [identifierProvider.GetProfileJson().ToString()],
            Links =
            [
                new LinkWebfingerModel()
                {
                    Href = identifierProvider.GetProfileJson().ToString(),
                    Rel = "self",
                    Type = "application/activity+json",
                },
            ],
            Subject = $"acct:{profile.Name}@{identifierProvider.GetBase().Host}",
        };

    private async Task<Profile> PublicProfileAsync(Context context)
    {
        return await context
                .Profiles.WhereUriMatch(identifierProvider.GetProfileJson())
                .SingleOrDefaultAsync() ?? throw new NotFoundException();
    }
}
