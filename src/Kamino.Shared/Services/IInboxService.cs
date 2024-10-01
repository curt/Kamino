using System.Text.Json.Nodes;

namespace Kamino.Shared.Services;

public interface IInboxService
{
    Task ReceiveAsync(JsonObject activity);
}
