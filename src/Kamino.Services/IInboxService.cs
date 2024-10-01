using System.Text.Json.Nodes;

namespace Kamino.Services;

public interface IInboxService
{
    Task ReceiveAsync(JsonObject activity);
}
