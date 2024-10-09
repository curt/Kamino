using System.Net.Http.Json;
using Kamino.Admin.Client.Models;

namespace Kamino.Admin.Client.Services;

public class PingsClientService(HttpClient httpClient) : IPingsService
{
    public async Task<IEnumerable<PingApiModel>> GetPingsAsync()
    {
        return await httpClient.GetFromJsonAsync<IEnumerable<PingApiModel>>("api/pings") ?? [];
    }
}
