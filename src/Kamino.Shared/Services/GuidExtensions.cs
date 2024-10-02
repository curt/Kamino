namespace Kamino.Shared.Services;

public static class GuidExtensions
{
    public static string ToId22(this Guid guid)
    {
        return Medo.Uuid7.FromGuid(guid!).ToId22String();
    }
}
