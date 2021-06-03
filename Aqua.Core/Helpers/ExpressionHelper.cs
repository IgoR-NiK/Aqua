using System;
using System.Linq.Expressions;

using Aqua.Core.Extensions;

namespace Aqua.Core.Helpers
{
    public static class ExpressionHelper
    {
        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var lambda = expression as LambdaExpression;
            
            var memberExpression = lambda.Body is UnaryExpression unaryExpression
                ? unaryExpression.Operand as MemberExpression
                : lambda.Body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException("Provide a lambda expression like 'it => it.PropertyName'");

            var memberInfo = memberExpression.Member;

            if (memberInfo.Name.IsNullOrEmpty())
                throw new ArgumentException("'Expression' did not provide a property name");

            return memberInfo.Name;
        }
    }
}