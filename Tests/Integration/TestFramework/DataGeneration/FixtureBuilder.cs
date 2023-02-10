using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace TestFramework.DataGeneration;


public class FixtureBuilder
{
    private readonly ISpecimenBuilder[] _builders;
    private readonly List<ISpecimenBuilderTransformation> _behaviors = new();
    private readonly IDictionary<Type, CustPair> _customizations = new Dictionary<Type, CustPair>();

    private int _repeatCount = 1;

    public FixtureBuilder(params ISpecimenBuilder[] builders)
    {
        _builders = builders ?? Array.Empty<ISpecimenBuilder>();
    }

    public FixtureBuilder WithRepeatCount(int repeatCount) => Apply(b => b._repeatCount = repeatCount);

    public FixtureBuilder Customize<T>(Func<IPostprocessComposer<T>, IPostprocessComposer<T>> cust)
    {
        return Customize<T>((c, f) => cust(c));
    }

    public FixtureBuilder Customize<T>(Func<IPostprocessComposer<T>, IFixture, IPostprocessComposer<T>> cust)
    {
        Type type = typeof(T);

        if (!_customizations.ContainsKey(type))
        {
            _customizations.Add(type, new CustPair { CustFunc = cust, CustAction = f => f.Customize<T>(c => cust(c, f)) });
        }
        else
        {
            Func<IPostprocessComposer<T>, IFixture, IPostprocessComposer<T>> prevCust
                = _customizations[type].CustFunc as Func<IPostprocessComposer<T>, IFixture, IPostprocessComposer<T>>;

            Func<IPostprocessComposer<T>, IFixture, IPostprocessComposer<T>> newCust
                = (c, f) => cust(prevCust(c, f), f);

            _customizations[type] = new CustPair { CustFunc = newCust, CustAction = f => f.Customize<T>(c => newCust(c, f)) };
        }

        return this;
    }

    /// <summary> Добавляет <see cref="ISpecimenBuilderTransformation"/> </summary>
    /// <param name="behavior"> <see cref="ISpecimenBuilderTransformation"/> </param>
    /// <returns> <see cref="FixtureBuilder"/> </returns>
    public FixtureBuilder AddBehavior(ISpecimenBuilderTransformation behavior)
    {
        _behaviors.Add(behavior);

        return this;
    }

    public IFixture Build()
    {
        IFixture fixture = new Fixture();

        fixture.Customize(new AutoMoqCustomization());

        foreach (ISpecimenBuilder customBuilder in _builders)
        {
            fixture.Customizations.Add(customBuilder);
        }

        fixture.RepeatCount = _repeatCount;

        fixture.Inject(true);
        fixture.Inject<DateTime?>(new DateTime(2001, 1, 1));
        fixture.Inject<DateOnly?>(new DateOnly(2001, 1, 1));
        fixture.Inject<int?>(100);
        fixture.Inject<decimal?>(100);

        foreach (var action in _customizations.Select(kv => kv.Value.CustAction))
        {
            action(fixture);
        }

        foreach (ISpecimenBuilderTransformation behavior in _behaviors)
        {
            fixture.Behaviors.Add(behavior);
        }

        return fixture;
    }

    private FixtureBuilder Apply(Action<FixtureBuilder> action)
    {
        action(this);
        return this;
    }

    private class CustPair
    {
        public object CustFunc { get; set; }

        public Action<IFixture> CustAction { get; set; }
    }
}
