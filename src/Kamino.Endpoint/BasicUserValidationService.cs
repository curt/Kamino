using AspNetCore.Authentication.Basic;

namespace Kamino.Endpoint;

public class BasicUserValidationService : IBasicUserValidationService
{
    public Task<bool> IsValidAsync(string username, string password) => Task.FromResult(false);
}
