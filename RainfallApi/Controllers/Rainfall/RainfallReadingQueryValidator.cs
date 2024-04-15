using FluentValidation;

namespace RainfallApi.Controllers.Rainfall;

/// <summary>
///     Validates the query parameters for a rainfall reading request
/// </summary>
public class RainfallReadingQueryValidator : AbstractValidator<RainfallReadingQuery>
{
    /// <summary>
    ///     Creates a new instance of the RainfallValidator
    /// </summary>
    public RainfallReadingQueryValidator()
    {
        RuleFor(x => x.StationId).NotEmpty();
        RuleFor(x => x.Count).InclusiveBetween(1, 100);
    }
}