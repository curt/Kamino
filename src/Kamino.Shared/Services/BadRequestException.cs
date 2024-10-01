namespace Kamino.Shared.Services;

public class BadRequestException(object? value = null) : Exception
{
    public object? Value { get; } = value;
}
