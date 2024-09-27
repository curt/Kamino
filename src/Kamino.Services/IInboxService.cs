using System.Text.Json.Nodes;
using Kamino.Models;

namespace Kamino.Services;

public interface IInboxService
{
    Task ReceiveAsync(JsonObject activity);
}
