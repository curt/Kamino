using Kamino.Validators;
using System.Text.Json;

namespace Kamino.Services;

public class InboxService
{
    public void Receive(JsonElement activity)
    {
        var validator = new InboxActivityValidator();
        var result = validator.Validate(activity);

        if (!result.IsValid)
        {
            throw new BadRequestException();
        }
    }
}
