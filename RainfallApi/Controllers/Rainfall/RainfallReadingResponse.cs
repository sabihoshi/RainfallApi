using Swashbuckle.AspNetCore.Annotations;

namespace RainfallApi.Controllers.Rainfall;

/// <summary>
///     Details of a rainfall reading
/// </summary>
[SwaggerSchema(Title = "Rainfall reading response")]
public class RainfallReadingResponse
{
    /// <summary>
    ///     A list of rainfall readings
    /// </summary>
    public required List<RainfallReading> Readings { get; set; }
}

/// <summary>
///     Details of a rainfall reading
/// </summary>
[SwaggerSchema(Title = "Rainfall reading")]
public class RainfallReading
{
    /// <summary>
    ///     The date and time the rainfall measurement was taken
    /// </summary>
    public DateTime DateMeasured { get; set; }

    /// <summary>
    ///     The amount of rainfall measured, in millimeters
    /// </summary>
    public decimal AmountMeasured { get; set; }
}