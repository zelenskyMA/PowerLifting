using AutoFixture.Kernel;
using System;
using System.Reflection;

namespace TestFramework.DataGeneration;

/// <summary>
/// Данные для генерации значения свойства в DTO
/// </summary>
public class PropertyData
{
    /// <summary>
    /// <see cref="PropertyInfo"/> обрабатываемого свойства.
    /// </summary>
    public PropertyInfo Info { get; set; }

    /// <summary>
    /// Контекст генерации данных для <see cref="FixtureBuilder"/>
    /// </summary>
    public ISpecimenContext Context { get; set; }

    /// <summary>
    /// Глобальный индекс смещения, чтобы Ид не пересекались.
    /// </summary>
    public int Offset { get; set; } = 0;

    /// <summary>
    /// Получение уникального значения Ид для текущего поля
    /// </summary>
    /// <returns>Ид</returns>
    public int GenerateAsId() => (Offset * 1000) + (Math.Abs(GetStableHashCode(Info.Name)) % 1000);

    private int GetStableHashCode(string str)
    {
        unchecked
        {
            int hash1 = 5381;
            int hash2 = hash1;

            for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
            {
                hash1 = (hash1 << 5) + hash1 ^ str[i];
                if (i == str.Length - 1 || str[i + 1] == '\0')
                {
                    break;
                }

                hash2 = (hash2 << 5) + hash2 ^ str[i + 1];
            }

            return Math.Abs(hash1 + (hash2 * 1566083941));
        }
    }
}
