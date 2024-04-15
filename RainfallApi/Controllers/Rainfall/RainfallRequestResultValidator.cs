using FluentValidation;
using RainfallApi.Client.Models;

namespace RainfallApi.Controllers.Rainfall;

/// <summary>
///     Validates the result of a rainfall reading request
/// </summary>
public class RainfallRequestResultValidator : AbstractValidator<RainfallRequestResult>
{
    /// <summary>
    ///     Creates a new instance of the RainfallRequestResultValidator
    /// </summary>
    public RainfallRequestResultValidator()
    {
        RuleFor(x => x.Items).NotEmpty()
           .WithMessage("No rainfall readings found for the specified stationId")
           .OverridePropertyName(nameof(RainfallReadingQuery.StationId));
    }
}