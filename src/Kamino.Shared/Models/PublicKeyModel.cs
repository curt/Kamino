using System;

namespace Kamino.Shared.Models;

public class PublicKeyModel
{
    public string? Id { get; set; }
    public string? Owner { get; set; }
    public string? PublicKeyPem { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object>? ExtraElements { get; set; }
}
