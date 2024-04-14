using System.Text.Json.Serialization;

namespace RainfallApi.Client.Models;

public class RainfallRequestResult
{
    public required Item[] Items { get; set; }

    public required Meta Meta { get; set; }

    [JsonPropertyName("@context")]
    public required Uri Context { get; set; }
}

public class Item
{
    public DateTimeOffset DateTime { get; set; }

    public decimal Value { get; set; }

    [JsonPropertyName("@id")]
    public Uri Id { get; set; }

    public Uri Measure { get; set; }
}

public class Meta
{
    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public required string Comment { get; set; }

    public required string Publisher { get; set; }

    public required string Version { get; set; }

    public required Uri Documentation { get; set; }

    public required Uri Licence { get; set; }

    public Uri[] HasFormat { get; set; } = [];
}