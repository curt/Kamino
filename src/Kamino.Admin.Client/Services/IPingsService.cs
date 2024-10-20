using Kamino.Admin.Client.Models;

namespace Kamino.Admin.Client.Services;

public interface IPingsService
{
    Task<IEnumerable<PingApiModel>> GetPingsAsync();

    Task<bool> SendPingAsync(string toUri);
}
