using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YueQianQian.Common.Extensions
{
    public static class MethodInfoExtensions
    {
        public static bool HasAttribute<T>(this MethodInfo method)
        {
            return method.GetCustomAttributes(typeof(T), false).FirstOrDefault() is T;
        }

        public static T GetAttribute<T>(this MethodInfo method) where T : Attribute
        {
            return method.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }

        public static bool IsAsync(this MethodInfo method)
        {
            return method.ReturnType == typeof(Task)
                || (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));

        }
    }
}
