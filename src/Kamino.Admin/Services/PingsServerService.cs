using Kamino.Admin.Client.Models;
using Kamino.Admin.Client.Services;
using Kamino.Shared.Repo;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Admin.Services;

public class PingsServerService(IDbContextFactory<NpgsqlContext> contextFactory) : IPingsService
{
    public async Task<IEnumerable<PingApiModel>> GetPingsAsync()
    {
        using var context = contextFactory.CreateDbContext();

        var pings = await context
            .Pings.Include(p => p.Actor)
            .Include(p => p.To)
            .Include(p => p.Pongs)
            .Select(p => new
            {
                PingUri = p.Uri,
                PingCreatedAt = p.CreatedAt,
                p.Pongs,
                ActorUri = p.Actor!.Uri,
                ActorDisplayName = p.Actor!.DisplayName,
                ActorIcon = p.Actor!.Icon,
                ToUri = p.To!.Uri,
                ToDisplayName = p.To!.DisplayName,
                ToIcon = p.To!.Icon,
            })
            .ToListAsync();

        return pings.Select(p => new PingApiModel
        {
            PingUri = p.PingUri?.ToString(),
            PingCreatedAt = p.PingCreatedAt,
            PongUri = p.Pongs.FirstOrDefault()?.Uri?.ToString(),
            PongCreatedAt = p.Pongs.FirstOrDefault()?.CreatedAt,
            ActorUri = p.ActorUri?.ToString(),
            ActorDisplayName = p.ActorDisplayName,
            ActorIcon = p.ActorIcon?.ToString(),
            ToUri = p.ToUri?.ToString(),
            ToDisplayName = p.ToDisplayName,
            ToIcon = p.ToIcon?.ToString(),
        });
    }

    public Task<PingApiModel> SendPing()
    {
        throw new NotImplementedException();
    }
}
