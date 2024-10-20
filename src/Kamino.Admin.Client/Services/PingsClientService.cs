using System.Net.Http.Json;
using Kamino.Admin.Client.Models;

namespace Kamino.Admin.Client.Services;

public class PingsClientService(HttpClient httpClient) : IPingsService
{
    public async Task<IEnumerable<PingApiModel>> GetPingsAsync()
    {
        return await httpClient.GetFromJsonAsync<IEnumerable<PingApiModel>>("api/pings") ?? [];
    }

    public async Task<bool> SendPingAsync(string toUri)
    {
        var response = await httpClient.PostAsJsonAsync(
            "api/pings",
            new PingApiModel { ToUri = toUri }
        );
        return response.IsSuccessStatusCode;
    }
}
