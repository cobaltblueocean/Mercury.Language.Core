using System;
using System.Linq;
using System.Collections.Generic;

namespace System
{
    public static class TypeExtension
    {
        /// <summary>
        /// Get all base type of the Type that given by this parameter
        /// </summary>
        /// <param name="self"></param>
        /// <returns>All Type</returns>
        public static IEnumerable<Type> GetBaseTypes(this Type self)
        {
            for (var baseType = self.BaseType; null != baseType; baseType = baseType.BaseType)
            {
                yield return baseType;
            }
        }

        public static Boolean  IsNullable(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;
            else
                return false;
        }

        public static Boolean IsComparable(this Type type)
        {
            return type.GetInterfaces().Any(x => x == typeof(IComparable));
        }

        public static Boolean HasGenericTypeArguments(this Type type, Type[] types)
        {
            if (!type.IsGenericType)
                return true;

            var result = true;

            Type[] genericTypes = type.GetGenericArguments();

            var _find = new Boolean[types.Length];
            _find.Fill(false);
            int _count = 0;

            foreach (var g in genericTypes)
            {
                foreach (var t in types)
                {
                    if (IsInheritType(g, t))
                    {
                        _find[_count] = true;
                    }
                }
                _count++;
            }
            if (_find.Any(f => !f))
                result = false;

            return result;
        }

        public static Boolean IsInheritType(this Type? type, Type targetType)
        {
            if ((type == null) || (type == typeof(Object)))
                    return false;

            if (type == targetType)
            {
                return true;
            }
            else
            {
                return IsInheritType(type.BaseType, targetType);
            }
        }
    }
}
