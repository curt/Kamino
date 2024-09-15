using Kamino.Models;

namespace Kamino.Services;

public interface IInboxService
{
    void Receive(ObjectInboxModel activity);
}
