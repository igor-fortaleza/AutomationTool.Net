using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Model.Generic.Extension
{
    public static class EnumerationExtension
    {
        public static string GetDescription(this Enum value) => value.GetType()
                .GetMember(value.ToString())
                .First()
                .GetCustomAttribute<DescriptionAttribute>()?
                .Description ?? string.Empty;

        public static T GetEnumValueFromDescription<T>(this string description)
        {
            Type type = typeof(T);
            if (!type.IsEnum)
                throw new InvalidOperationException();
            foreach (FieldInfo field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute((MemberInfo)field, typeof(DescriptionAttribute)) is DescriptionAttribute customAttribute && customAttribute.Description == description)
                    return (T)field.GetValue((object)null);
                if (field.Name == description)
                    return (T)field.GetValue((object)null);
            }
            throw new ArgumentException("Elemento não encontrado", nameof(description));
        }

        public static string GetAttributeText(this Enum enumValue, Type enumType)
        {
            MemberInfo element = ((IEnumerable<MemberInfo>)enumType.GetMember(enumValue.ToString())).First<MemberInfo>();
            string str;
            if (element != (MemberInfo)null && element.CustomAttributes.Any<CustomAttributeData>())
            {
                DisplayAttribute customAttribute = element.GetCustomAttribute<DisplayAttribute>();
                str = customAttribute != null ? customAttribute.Name : enumValue.ToString();
            }
            else
                str = enumValue.ToString();
            return str;
        }
    }
}
