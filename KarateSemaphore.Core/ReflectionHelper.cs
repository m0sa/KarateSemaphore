using System;
using System.Linq.Expressions;
using System.Reflection;

namespace KarateSemaphore.Core
{
    public class ReflectionHelper
    {
        private static PropertyInfo GetPropertyInternal(LambdaExpression p)
        {
            MemberExpression memberExpression;
            var unary = p.Body as UnaryExpression;
            if (null != unary)
            {
                memberExpression = (MemberExpression)unary.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)p.Body;
            }

            return (PropertyInfo)(memberExpression).Member;
        }

        public static string GetPropertyName<T>(Expression<Func<T>> p)
        {
            return GetProperty(p).Name;
        }

        public static PropertyInfo GetProperty<T>(Expression<Func<T>> p)
        {
            return GetPropertyInternal(p);
        }
    }
}
