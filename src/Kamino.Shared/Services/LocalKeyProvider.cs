using Kamino.Shared.Entities;
using Kamino.Shared.Repo;
using Microsoft.EntityFrameworkCore;
using SevenKilo.HttpSignatures;

namespace Kamino.Shared.Services;

public class LocalKeyProvider(IDbContextFactory<NpgsqlContext> contextFactory) : IKeyProvider
{
    public async Task<KeyModel?> GetKeyModelByKeyIdAsync(string keyId)
    {
        using var context = contextFactory.CreateDbContext();

        var profile = await context
            .Profiles.Where(p => p.Uri == Constants.LocalProfileUri)
            .FirstOrDefaultAsync();
        if (profile != null && profile.PublicKey != null)
        {
            return new KeyModel(profile.PublicKey, profile.PrivateKey);
        }

        return null;
    }
}
