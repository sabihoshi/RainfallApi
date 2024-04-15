using Refit;

namespace RainfallApi.Client;

public class RainfallApi
{
    public static IRainfallApi Create()
        => RestService.For<IRainfallApi>("https://environment.data.gov.uk/flood-monitoring");
}