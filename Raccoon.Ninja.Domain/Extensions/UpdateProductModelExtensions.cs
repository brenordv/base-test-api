using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.Domain.Extensions;

public static class UpdateProductModelExtensions
{
    public static IDictionary<string, object> Parse(this UpdateProductModel updateModel)
    {
        var dict = new Dictionary<string, object>();
        var type = updateModel.GetType();
        var props = type.GetProperties();

        foreach (var propertyInfo in props)
        {
            var propName = propertyInfo.Name;

            if (propName.Equals(nameof(updateModel.Id))) continue;

            var value = propertyInfo.GetValue(updateModel);

            if (value == null) continue;

            dict.Add(propName, value);
        }

        return dict;
    }
}