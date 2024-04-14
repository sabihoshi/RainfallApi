namespace RainfallApi.Controllers.Rainfall;

/// <summary>
///  Details of a rainfall reading
/// </summary>
public class RainfallReadingResponse
{
    /// <summary>
    ///     A list of rainfall readings
    /// </summary>
    public required List<RainfallReading> Readings { get; set; }
}