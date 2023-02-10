using AutoFixture.Kernel;
using System;

namespace TestFramework.DataGeneration;

internal class CustomPropertyBuilder : PropertyBuilder
{
    protected override object Create(PropertyData propData)
    {
        string propName = propData.Info.Name;
        string propNameLower = propName.ToLower();
        Type propType = propData.Info.PropertyType;

        if (propName.StartsWith("Id"))
        {
            return propData.GenerateAsId();
        }
       
        if (propNameLower.Contains("mail"))
        {
            return $"{propNameLower}@testserver.com";
        }

        if (propType == typeof(string))
        {
            return $"Test {propName}";
        }
        
        /*
        if (propType.IsGenericType
        && propType.GetGenericTypeDefinition() == typeof(IList<>)
        && propType.GetGenericArguments().FirstOrDefault() is Type listItemType
        && IsDictionaryItem(listItemType))
        {
            IEnumerable items = propData.Context.Resolve(propType) as IEnumerable;
            var codeProp = listItemType.GetProperty(nameof(DictionaryItem.Code));

            foreach (object item in items)
            {
                codeProp.SetValue(item, $"{propName.TrimEnd('s')}_TEST");
            }

            return items;
        }

        if (typeof(ContactPerson).IsAssignableFrom(propType))
        {
            object person = propData.Context.Resolve(propType);

            foreach (var personProperty in propType.GetProperties())
            {
                if (personProperty.Name.Contains("mail"))
                {
                    personProperty.SetValue(person, $"{propNameLower}@testserver.com");
                }
                else if (personProperty.PropertyType == typeof(string))
                {
                    personProperty.SetValue(person, $"Test {propName} {personProperty.Name}");
                }
                else
                {
                    personProperty.SetValue(person, propData.Context.Resolve(personProperty.PropertyType));
                }
            }

            return person;
        }
        */

        return new NoSpecimen();
    }
}
