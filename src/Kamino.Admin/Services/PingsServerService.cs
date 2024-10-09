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
            .ToListAsync();
        return pings.Select(p => new PingApiModel
        {
            PingUri = p.Uri?.ToString(),
            PingCreatedAt = p.CreatedAt,
            PongUri = p.Pongs.FirstOrDefault()?.Uri?.ToString(),
            PongCreatedAt = p.Pongs.FirstOrDefault()?.CreatedAt,
            ActorUri = p.Actor?.Uri?.ToString(),
            ActorDisplayName = p.Actor?.DisplayName,
            ActorIcon = p.Actor?.Icon?.ToString(),
            ToUri = p.To?.Uri?.ToString(),
            ToDisplayName = p.To?.DisplayName,
            ToIcon = p.To?.Icon?.ToString(),
        });
    }

    public Task<PingApiModel> SendPing()
    {
        throw new NotImplementedException();
    }
}
