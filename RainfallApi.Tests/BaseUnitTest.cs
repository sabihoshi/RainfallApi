using System.Reflection;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using NSubstitute;

namespace RainfallApi.Tests;

public abstract class BaseUnitTest
{
    protected BaseUnitTest()
    {
        Fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

        var throwingRecursionBehaviors = Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList();
        foreach (var behavior in throwingRecursionBehaviors)
        {
            Fixture.Behaviors.Remove(behavior);
        }

        Fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
    }

    protected IFixture Fixture { get; } = new Fixture();

    protected T Get<T>() where T : class => Fixture.Create<T>();
}

public abstract class BaseUnitTest<TUnit> : BaseUnitTest where TUnit : class
{
    private TUnit? _unit;

    protected BaseUnitTest()
    {
        foreach (var (mock, mockType) in ListMockableTypesForSystemUnderTest())
        {
            InjectObject(Fixture, mock, mockType);
        }
    }

    protected TUnit Unit => _unit ??= Fixture.Build<TUnit>().OmitAutoProperties().Create();

    private static bool IsMockableParameter(ParameterInfo info) => !info.ParameterType.IsValueType;

    private IEnumerable<(object MockedObject, Type ParameterType)> ListMockableTypesForSystemUnderTest()
        => typeof(TUnit).GetConstructors()
           .Single()
           .GetParameters()
           .Where(IsMockableParameter)
           .Select(param => (Substitute.For([param.ParameterType], []), param.ParameterType));

    private static void InjectObject(IFixture fixture, object objectToInject, Type realTypeOfObject)
    {
        var injectMethod = typeof(FixtureRegistrar)
           .GetMethods(BindingFlags.Public | BindingFlags.Static)
           .Single(s => s.Name == nameof(FixtureRegistrar.Inject));

        var genericInject = injectMethod.MakeGenericMethod(realTypeOfObject);
        _ = genericInject.Invoke(null, [fixture, objectToInject]);
    }
}