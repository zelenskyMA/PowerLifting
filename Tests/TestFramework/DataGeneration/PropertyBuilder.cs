using AutoFixture.Kernel;
using System.Reflection;
using System.Threading;

namespace TestFramework.DataGeneration;

public abstract class PropertyBuilder : ISpecimenBuilder
{
    private static int _currentIdOffset = 0;
    private int _idOffset = Interlocked.Increment(ref _currentIdOffset);

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (request is PropertyInfo propInfo)
        {
            PropertyData data = new()
            {
                Info = propInfo,
                Context = context,
                Offset = _idOffset
            };

            return Create(data);
        }

        return new NoSpecimen();
    }

    /// <summary> Реализация обработки свойств модели для генерации ее тестовых данных </summary>
    /// <param name="propData">Обрабатываемое свойство в модели</param>
    /// <returns>Значение свойства</returns>
    protected abstract object Create(PropertyData propData);
}
