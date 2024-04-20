using System.Text.Json.Serialization;

namespace RainfallApi.Client.Models;

public class Item
{
    public DateTimeOffset DateTime { get; set; }

    public decimal Value { get; set; }

    [JsonPropertyName("@id")]
    public required Uri Id { get; set; }

    public required Uri Measure { get; set; }
}