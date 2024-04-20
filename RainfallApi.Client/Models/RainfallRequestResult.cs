using System.Text.Json.Serialization;

namespace RainfallApi.Client.Models;

public class RainfallRequestResult
{
    public required Item[] Items { get; set; }

    public required Meta Meta { get; set; }

    [JsonPropertyName("@context")]
    public required Uri Context { get; set; }
}