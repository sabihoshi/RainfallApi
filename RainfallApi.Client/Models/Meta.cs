namespace RainfallApi.Client.Models;

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