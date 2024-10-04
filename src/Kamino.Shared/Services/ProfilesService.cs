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
            Id = identifierProvider.GetProfileJson(),
            Type = "Person",
            Inbox = identifierProvider.GetPathJson("inbox"),
            Outbox = identifierProvider.GetPathJson("outbox"),
            Followers = identifierProvider.GetPathJson("followers"),
            Following = identifierProvider.GetPathJson("following"),
            Name = profile.DisplayName,
            PreferredUsername = profile.Name,
            Summary = profile.Summary,
            Url = identifierProvider.GetProfileHtml(),
            PublicKey = new
            {
                Id = identifierProvider.GetKeyId(),
                Owner = identifierProvider.GetProfileJson(),
                PublicKeyPem = profile.PublicKey,
            },
        };

    private ProfileWebfingerModel CreatePublicWebfingerModel(Profile profile) =>
        new()
        {
            Aliases = [identifierProvider.GetProfileJson()],
            Links =
            [
                new LinkWebfingerModel()
                {
                    Href = identifierProvider.GetProfileJson(),
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
